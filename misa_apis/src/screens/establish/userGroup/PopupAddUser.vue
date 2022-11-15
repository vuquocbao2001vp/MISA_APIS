<template>
  <div class="popup" :class="{ hide: isHide }">
    <div class="popup-form">
      <div class="popup-header">
        <h1 class="popup-header-title">Thêm thành viên</h1>
        <div class="popup-header-close flex" @click="closePopup">
          <div class="icon-close"></div>
        </div>
      </div>
      <div class="header-content">
        <div class="header-content-select flex">
          <div class="select-input flex" >
            <div class="icon-textbox"></div>
            <DxTextBox
              class="control-textbox"
              placeholder="Tìm kiếm người dùng"
              value-change-event="keyup"
              @value-changed="getTextSearch"
              v-model:value="textSearch"
            />
          </div>
          <div class="select-tagbox">
            <div class="job-title-position"></div>
            <DxTagBox
              class="control-tagbox"
              :items="jobPosition"
              display-expr="JobPositionName"
              value-expr="JobPositionID"
              placeholder="Tất cả chức vụ"
              noDataText="Không có dữ liệu"
              v-model:value="listJobPositionSearch"
              @value-changed="getJobSearch"
            />
          </div>
          <div class="select-dropdown">
            <div class="icon-treeview"></div>
            <DxDropDownBox
              class="control-treeview"
              :data-source="organizationUnit"
              value-expr="OrganizationUnitID"
              display-expr="OrganizationUnitName"
              placeholder="Chọn đơn vị"
              v-model:value="treeBoxValue"
              v-model:opened="isTreeBoxOpened"
              @value-changed="syncTreeViewSelection()"
            >
              <template #content="{}">
                <DxTreeView
                  search-enabled="true"
                  :searchEditorOptions="{ placeholder: 'Tìm kiếm' }"
                  :ref="treeViewRefName"
                  :data-source="organizationUnit"
                  :select-by-click="true"
                  data-structure="plain"
                  key-expr="OrganizationUnitID"
                  parent-id-expr="ParentID"
                  selection-mode="single"
                  display-expr="OrganizationUnitName"
                  @content-ready="$event.component.selectItem(treeBoxValue)"
                  @item-selection-changed="
                    treeView_itemSelectionChanged($event)
                  "
                  @item-click="onTreeItemClick()"
                />
              </template>
            </DxDropDownBox>
          </div>
        </div>
        <div
          class="header-content-diselect flex"
          v-if="listUserSelectedID.length > 0"
        >
          <div class="selected-quantity">
            <span>{{ listUserSelectedID.length }}</span> đang chọn
          </div>
          <div class="disselect-span" @click="diselectAll">Bỏ chọn</div>
        </div>
      </div>
      <div class="popup-grid">
        <DxDataGrid
          :data-source="userOfGroupSelected"
          :allow-column-reordering="true"
          :allow-column-resize="true"
          :hover-state-enabled="true"
          key-expr="UserID"
          :selected-row-keys="userSelectedIds"
          @option-changed="onSelectionChanged"
        >
          <DxPaging :enabled="false" />
          <DxColumn data-field="FullName" caption="Họ và tên" />
          <DxColumn data-field="JobPositionName" caption="Chức vụ" />
          <DxColumn data-field="OrganizationUnitName" caption="Phòng ban" />
          <DxColumn data-field="OrganizationName" caption="Đơn vị" />
          <DxColumn data-field="Email" caption="Email" />
          <DxSelection
            :deferred="true"
            mode="multiple"
            show-check-boxes-mode="always"
            select-all-mode="page"
          />
        </DxDataGrid>
        <BasePaging
          :totalRecords="totalRecords"
          :totalPages="totalPages"
          :pageSize="pageSize"
          :pageNumber="pageNumber"
          :recordStart="recordStart"
          :recordEnd="recordEnd"
          @getData="loadMemberPaging"
        />
      </div>
      <div class="popup-footer flex">
        <div class="popup-footer-button flex">
          <div @click="closePopup">
            <BaseButton buttonType="white" buttonName="Hủy bỏ" />
          </div>
          <div class="btn" @click="updateMemberOfGroup"><BaseButton buttonType="regular" buttonName="Đồng ý" /></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mapMutations, mapActions, mapGetters } from "vuex";
import BaseApi from '../../../constants/BaseApi.js'

export default {
  props: ["isHide", "groupSelected", "userSelected"],
  data() {
    return {
      timeout: null,
      textSearch: "",
      listJobPositionSearch: [],
      listJob: "",
      userOfGroupSelected: [],
      userSelectedIds: [],
      listUserSelectedID: [],
      totalRecords: null,
      totalPages: null,
      pageSize: null,
      pageNumber: null,
      recordStart: null,
      recordEnd: null,
      treeViewRefName: "tree-view",
      treeBoxValue: null,
      isTreeBoxOpened: false,
    };
  },
  computed: {
    ...mapGetters(["jobPosition"]),
    organizationUnit(){
      let selectNone = {
        OrganizationUnitID: "0",
        OrganizationUnitName: "- Không chọn -"
      }
      return [selectNone, ...this.$store.getters.organizationUnit];
    }
  },
  mounted() {
    this.getJobPositionData();
    this.getOrganizationUnitData();
  },
  watch: {
    isHide: function (value) {
      if (!value) {
        this.getUserDataOfGroupPaging(50, 1);
        var listMemberId = this.userSelected?.map(function (member) {
          return member["UserID"];
        });
        this.userSelectedIds = listMemberId;
      } else {
        this.textSearch = null;
        this.listJobPositionSearch = [];
        this.listJob = "";
        this.treeBoxValue = null;
      }
    },
    /**
     * Khi từ khóa tìm kiếm thay đổi, load lại paging
     * Author: VQBao - 8/9/2022
     */
    textSearch: function (value) {
      try {
        this.getUserDataOfGroupPaging(
          this.pageSize,
          1,
          "",
          value,
          this.listJob,
          this.groupSelected.MISACode
        );
      } catch (error) {
        console.log(error);
      }
    },
  },
  methods: {
    ...mapMutations(["getJobPosition", "getOrganizationUnit"]),
    ...mapActions(["getJobPositionData", "getOrganizationUnitData"]),
    /**
     * Phân trang người dùng
     * Author: VQBao - 20/9/2022
     */
    getUserDataOfGroupPaging(pageSize, pageNumber, userGroupId, searchByText, searchByPosition, MISACode) {
      document.getElementById("loader").style.display = "block";
      try {
        axios.get(
            BaseApi.UserGroupApi+`/MemberOfGroupPaging`,{
              params: {
                pageSize: pageSize,
                pageNumber: pageNumber,
                userGroupId: userGroupId,
                searchByText: searchByText,
                searchByPosition: searchByPosition,
                MISACode: MISACode,
              },
            }
          )
          .then((response) => {
            this.userOfGroupSelected = response.data.Data;
            this.totalRecords = response.data.TotalRecords;
            this.totalPages = response.data.TotalPages;
            this.recordStart = response.data.RecordStart;
            this.recordEnd = response.data.RecordEnd;
            this.pageSize = response.data.PageSize;
            this.pageNumber = response.data.PageNumber;
            document.getElementById("loader").style.display = "none";
          });
      } catch (error) {
        document.getElementById("loader").style.display = "none";
        console.log(error);
      }
    },
    /**
     * Tắt popup khi click vào icon tắt
     * Author: VQBao - 30/8/2022
     */
    closePopup() {
      this.$emit("hidePopup", true);
      console.log(this.userSelected);
    },
    /**
     * Lấy ra từ khóa tìm kiếm sau khi nhập xong input
     * Author; VQBao - 8/9/2022
     */
    getTextSearch(data) {
      clearTimeout(this.timeout);
      var self = this;
      this.timeout = setTimeout(function () {
        self.textSearch = data.value;
      }, 500);
    },
    /**
     * Phương thức thực hiện phân trang người dùng
     * Author: VQBao - 8/9/2022
     */
    loadMemberPaging(pageSize, pageNumber) {
      try {
        this.getUserDataOfGroupPaging(
          pageSize,
          pageNumber,
          "",
          this.textSearch,
          this.listJob,
          this.groupSelected.MISACode
        );
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Khi cây đơn vị thay đổi giá trị, ô input nhận giá trị đó
     */
    syncTreeViewSelection() {},

    /**
     * Khi phần tử được chọn của cây đơn vị thay đổi, tìm kiếm và phân trang theo giá trị đó
     * Author: VQBao - 15/9/2022
     */
    treeView_itemSelectionChanged(e) {
      this.treeBoxValue = e.component.getSelectedNodeKeys();
      var a = e.component.getSelectedNodes();
      try {
        this.getUserDataOfGroupPaging(
          this.pageSize,
          1,
          "",
          this.textSearch,
          this.listJob,
          a[0].itemData.MISACode
        );
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Khi click chọn 1 phần tử trong cây đơn vị, sau khi lấy ra giá trị thì đóng cây đơn vị
     * Author: VQBao - 15/9/2022
     */
    onTreeItemClick() {
      this.isTreeBoxOpened = false;
      this.treeBoxValue = "";
    },

    /**
     * Tìm kiếm theo chức vụ
     * Author: VQBao - 15/9/2022
     */
    getJobSearch() {
      this.listJob = this.listJobPositionSearch.join(",");
      try {
        this.getUserDataOfGroupPaging(
          this.pageSize,
          1,
          "",
          this.textSearch,
          this.listJob,
          this.groupSelected.MISACode
        );
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Phương thức lấy về mảng id tất cả người dùng đang chọn
     * Author: VQBao - 20/9/2022
     */
    onSelectionChanged(e) {
      if (e.name == "selectionFilter") {
        this.listUserSelectedID = [];
        if (e.value != null) {
          if (e.value[0] == "UserID") {
            this.listUserSelectedID.push(e.value[2]);
          } else {
            for (let i = 0; i < e.value.length; i += 2) {
              this.listUserSelectedID.push(e.value[i][2]);
            }
          }
        }
      }
    },
    /**
     * Khi bấm 'Bỏ chọn', bỏ chọn tất cả người dùng đang chọn
     * Author: VQBao - 20/9/2022
     */
    diselectAll() {
      this.userSelectedIds = [];
      this.listUserSelectedID = [];
    },
    /**
     * Cập nhật lại thành viên trong nhóm
     * Author: VQBao - 20/9/2022
     */
    updateMemberOfGroup(){
      document.getElementById("loader").style.display = "block";
      if(this.listUserSelectedID.length > 0){
        try {
          axios.put(`https://localhost:44382/api/v1/UserGroups/UpdateMemberOfGroup/`+this.groupSelected.UserGroupID, this.listUserSelectedID)
          .then(() => {
            this.$emit("hidePopup", true);
            document.getElementById("loader").style.display = "none";
            this.$emit("hideToast", "Cập nhật nhóm người dùng thành công")
          })
        } catch (error) {
          console.log(error);
          this.$emit("hidePopup", true);
        }
      }
    },
  },
};
</script>

<style scoped>
.hide {
  display: none !important;
}
.popup {
  transition: all 0.2s;
  width: 100%;
  height: 100%;
  position: fixed;
  top: 0;
  left: 0;
  z-index: 100;
  display: flex;
  align-items: flex-start;
  align-items: center;
  justify-content: center;
  opacity: 1;
  z-index: 100;
  background: rgba(0, 0, 0, 0.4);
}
.popup-form {
  background: rgb(255, 255, 255);
  width: 992px;
  min-width: 500px;
  height: 85vh;
  border-radius: 6px;
  box-sizing: border-box;
}
.popup-header {
  height: 53px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-radius: 8px 8px;
  position: relative;
  padding: 24px;
  padding-bottom: 0;
  min-height: 53px !important;
  box-sizing: border-box;
}
.popup-header-title {
  font-size: 24px;
  letter-spacing: 0.384px !important;
  font-weight: 700;
  color: var(--text-primary-color);
  margin: 0;
}
.popup-header-close {
  height: 36px;
  width: 36px;
  justify-content: center;
  position: absolute;
  right: 16px;
  border-radius: 50%;
  cursor: pointer;
}
.popup-header-close:hover {
  background-color: rgb(220, 220, 220);
}
.header-content {
  margin: 24px 0;
  padding: 0 24px;
  box-sizing: border-box;
}
.select-input {
  width: 305px;
  box-sizing: border-box;
  align-items: center;
  position: relative;
}
.icon-textbox {
  position: absolute;
  content: "";
  top: 9px;
  left: 10px;
  width: 18px;
  height: 18px;
  mask: url("../../../assets/Icons/Icon_request.0a9f1483.svg") no-repeat -291px -4px;
  background-color: #9fa4b4!important;
  z-index: 1;
}
.control-textbox {
  padding-left: 28px;
  width: 100% !important;
}
.select-tagbox {
  width: 307px;
  margin-left: 12px;
  box-sizing: border-box;
  align-items: center;
  position: relative;
}
.job-title-position {
  position: absolute;
  content: "";
  top: 7px;
  left: 6px;
  width: 24px;
  height: 24px;
  mask: url("../../../assets/Icons/Icon_request.0a9f1483.svg") no-repeat -574px -23px;
  background-color: #65696e;
  z-index: 1;
}
.control-tagbox {
  padding-left: 28px;
}
.select-dropdown {
  width: 305px;
  margin-left: 12px;
  box-sizing: border-box;
  align-items: center;
  position: relative;
}
.icon-treeview {
  position: absolute;
  content: "";
  top: 8px;
  left: 8px;
  width: 16px;
  height: 18px;
  mask: url("../../../assets/Icons/Icon_request.0a9f1483.svg") no-repeat -554px -24px;
  background-color: #65696e;
  z-index: 1;
}
.control-treeview {
  padding-left: 24px;
}
.header-content-diselect {
  margin: 16px 0;
}
.selected-quantity {
  margin: 0 16px 0 0;
}
.disselect-span {
  margin: 0 8px 0 0;
  color: #13b2d8;
  font-weight: 500;
  cursor: pointer;
}
.popup-footer {
  height: 61px;
  border-radius: 0 0 4px 4px;
  border-top: 1px solid #dadce3;
  justify-content: flex-end;
  background: #f2f2f2;
  padding: 12px 24px;
  border-radius: 0 0 8px 8px;
}
.popup-grid {
  height: calc(100% - 233px);
  padding: 0 24px;
  box-sizing: border-box;
}
#btnUpdate {
  width: fit-content;
  height: 36px;
  box-sizing: border-box;
}
.btn {
  width: 96px;
  height: 36px;
}
</style>