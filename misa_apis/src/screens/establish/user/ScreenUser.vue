<template>
  <div class="content-header flex" id="userScreen">
    <span class="header-text">Người dùng</span>
    <div class="header-toolbar flex">
      <div class="toolbar-select-box">
        <DxSelectBox
          :data-source="userGroups"
          display-expr="UserGroupName"
          value-expr="UserGroupName"
          placeholder="Chọn nhóm người dùng"
          noDataText="Không có dữ liệu"
          @value-changed="getGroupItemSearch"
        />
      </div>
      <div class="toolbar-input flex">
        <div class="icon-textbox"></div>
        <DxTextBox
          class="control-textbox"
          placeholder="Tìm kiếm họ và tên, email/số điện thoại"
          value-change-event="keyup"
          @value-changed="getTextSearch"
        />
      </div>
      <div @click="hideDialogImport(false)"><BaseButton buttonType="regular" buttonName="Nhập khẩu" /></div>
      <div @click="exportToExcel">
        <BaseButton buttonType="regular" buttonName="Xuất khẩu" />
      </div>

      <div class="toolbar-setting flex">
        <div class="setting flex"><div class="icon-setting" @click="reorderColumn"></div></div>
        <div class="popup-resize-col" :class="{ hide: isHideReorderPopup }">
          <div class="arrow"></div>
          <div class="popup-resize-content">
            <div class="resize-content-header flex">
              <span class="header-title">Tùy chỉnh cột</span>
              <div class="header-icon flex">
                <div class="icon-refresh" @click="saveDefaultOption" title="Lấy lại mặc định"></div>
              </div>
            </div>
            <div class="col-block">
              <div class="col-item">
                <DxList
                  :data-source="listGrid"
                  :repaint-changes-only="false"
                  :search-enabled="true"
                  item-template="list-item"
                  :searchEditorOptions="{ placeholder: 'Tìm kiếm' }"
                >
                  <template #list-item="{ data: item }">
                    <DxCheckBox
                      v-model:value="item.IsVisible"
                      :text="item.Caption"
                    />
                  </template>
                  <DxItemDragging
                    :allow-reordering="true"
                    :on-drag-end="onDragEnd"
                  />
                </DxList>
              </div>
            </div>
            <div class="popup-footer flex">
              <div @click="saveUserOption"><BaseButton buttonType="regular" buttonName="Lưu" /></div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="content-grid">
    <DxDataGrid
      :data-source="users"
      :allow-column-reordering="true"
      :allow-column-resize="true"
      :hover-state-enabled="true"
      column-resizing-mode="widget"
      :selection="{ mode: 'single' }"
      @row-click="rowOnClick"
      no-data-text="Không có dữ liệu để hiển thị"
    >
      <DxPaging :enabled="false" />
      <DxColumn
        v-for="option in userGrid.userOption"
        :key="option"
        :data-field="option.FieldName"
        :visible="option.IsVisible"
        :caption="option.Caption"
        :width="option.Width"
        :alignment="(option.FieldName === 'Status' && 'center') || 'left'"
        :cell-template="option.FieldName === 'Status' && 'cellTemplateStatus'"
      />
      <template #cellTemplateStatus="{ data }">
        <div class="custom-column-status" v-if="data.value == '1'">
          Đang hoạt động
        </div>
        <div class="custom-column-status red" v-else>Chưa kích hoạt</div>
      </template>
      <DxColumn cell-template="cellTemplateButton" :width="50" />
      <template #cellTemplateButton="{ data }">
        <div class="cell-button flex" @click.stop="deleteUserOnClick(data)">
          <div class="icon-delete" title="Xóa"></div>
        </div>
      </template>
    </DxDataGrid>
  </div>
  <BasePaging
    :totalRecords="userPaging.totalRecords"
    :totalPages="userPaging.totalPages"
    :pageSize="userPaging.pageSize"
    :pageNumber="userPaging.pageNumber"
    :recordStart="userPaging.recordStart"
    :recordEnd="userPaging.recordEnd"
    @getData="loadUserPaging"
  />
  <DialogImport
    :isHideDialogImport="isHideDialogImport"
    @hideDialogImport="hideDialogImport"
    @hideToast="hideToast"
    @loadData="loadData"
  />
  <DialogUserDetail
    :isShow="isShowDialog"
    @showDialog="showDialog"
    :userSelectedId="userSelectedId"
    @loadData="loadData"
    @hideToast="hideToast"
  />
  <BasePopup
    :isHidePopup="isHidePopup"
    title="Thông báo"
    :text="popupText"
    @confirmPopupAction="deleteUser"
    @hidePopup="hidePopup"
  />
  <BaseToast
    :isHideToast="isHideToast"
    :text="toastText"
  />
  
</template>
<script>
import DialogUserDetail from "./DialogUserDetail.vue";
import DialogImport from "./DialogImport.vue";
import { mapMutations, mapActions, mapGetters } from "vuex";
import axios from "axios";
export default {
  components: {
    DialogUserDetail,
    DialogImport
  },
  data() {
    return {
      toastText: "",
      popupText: "",
      isShowDialog: false,
      isHideReorderPopup: true,
      isHidePopup: true,
      isHideDialogImport: true,
      textSearch: "",
      groupItemSearch: "",
      timeout: null,
      userSelectedId: null,
      userIdToDelete: null,
      isHideToast: true,
      groups: [],
      userGridId: "1c142e9d-5b5b-7f1f-6329-a2925c0f334d",
      listGrid: [],
    };
  },
  computed: {
    ...mapGetters(["users", "userPaging"]),
    userGroups(){
      let selectAll = {
        UserGroupID: "",
        UserGroupName: "Tất cả",
      }
      return [selectAll, ...this.$store.getters.userGroups];
    },
    userGrid: {
      get() {
        return this.$store.getters.userGrid;
      },
      set(value) {
        this.$store.commit("getUserGrid", value);
      }
    }
  },
  mounted() {
    this.getUserData({
      pageSize: 50,
      pageNumber: 1,
      searchByText: "",
      searchByGroup: "",
    });
    this.getAllGroupData();
    this.getUserGridData(this.userGridId);
  },
  watch: {
    isHideReorderPopup: function(value){
      if(!value && this.listGrid.length == 0){
        this.listGrid = this.userGrid.userOption;
      }
    },
    /**
     * Khi từ khóa tìm kiếm thay đổi, load lại paging
     * Author: VQBao - 8/9/2022
     */
    textSearch: function (value) {
      try {
        this.getUserData({
          pageSize: this.userPaging.pageSize,
          pageNumber: 1,
          searchByText: value,
          searchByGroup: this.groupItemSearch,
        });
      } catch (error) {
        console.log(error);
      }
    },
  },
  methods: {
    ...mapMutations([
      "getUserPaging",
      "getUserById",
      "getAllGroup",
      "getUserGrid",
    ]),
    ...mapActions([
      "getUserData",
      "getUserDataById",
      "getAllGroupData",
      "getUserGridData",
    ]),

    /**
     * Khi di chuyển cột ở popup tùy chỉnh cột, thay đổi lại vị trí của cột trong mảng danh sách cột
     * Author: VQBao - 15/9/2022
     */
    onDragEnd(e) {
      console.log(e, e.fromIndex, e.toIndex);
      this.listGrid.splice(e.toIndex, 0, this.listGrid.splice(e.fromIndex, 1)[0]);
    },

    /**
     * Khi click lưu, thực hiện gọi api lưu lại tùy chỉnh cột
     * Author: VQBao - 15/9/2022
     */
    async saveUserOption(){
      try {
        await axios.put(`https://localhost:44382/api/v1/Users/SaveUserOption/`+ this.userGridId, this.listGrid)
        .then(() => {
          this.isHideReorderPopup = true;
          this.getUserGridData(this.userGridId);
          this.loadData();
        })
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy lại tùy chỉnh cột mặc định
     * Author: VQBao - 15/9/2022
     */
    saveDefaultOption(){
      this.listGrid = this.userGrid.defaultOption;
      this.saveUserOption();
    },

    /**
     * Phương thức thực hiện phân trang người dùng
     * Author: VQBao - 8/9/2022
     */
    loadUserPaging(pageSize, pageNumber) {
      try {
        this.getUserData({
          pageSize: pageSize,
          pageNumber: pageNumber,
          searchByText: this.textSearch,
          searchByGroup: this.groupItemSearch,
        });
        this.userSelectedId = null;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Load lại data đầy đủ thông tin paging khi có thay đổi về dữ liệu
     * Author: VQBao - 10/8/2022
     */
    loadData() {
      try {
        this.getUserData({
          pageSize: this.userPaging.pageSize,
          pageNumber: this.userPaging.pageNumber,
          searchByText: this.textSearch,
          searchByGroup: this.groupItemSearch,
        });
        this.userSelectedId = null;
      } catch (error) {
        console.log(error);
      }
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
     * Khi chọn nhóm người dùng, phân trang người dùng lọc theo nhóm người dùng đó
     * Author: VQBao - 8/9/2022
     */
    getGroupItemSearch(data) {
      if(data.value == "Tất cả"){
        this.groupItemSearch = "";
      } else {
        this.groupItemSearch = data.value;
      }
      
      try {
        this.getUserData({
          pageSize: this.userPaging.pageSize,
          pageNumber: 1,
          searchByText: this.textSearch,
          searchByGroup: this.groupItemSearch,
        });
        this.userSelectedId = null;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Khi click vào hàng trong bảng thì cửa sổ thông tin chi tiết hiện ra
     * Author: VQBao - 30/8/2022
     */
    rowOnClick(userRow) {
      this.userSelectedId = userRow.data.UserID;
      this.showDialog(true);
    },

    /**
     * Ẩn hiện cửa sổ thông tin chi tiết người dùng
     * Author: VQBao - 30/8/2022
     */
    showDialog(isShow) {
      this.isShowDialog = isShow;
    },

    /**
     * Ẩn hiện popup thông báo
     * Author: VQBao - 10/8/2022
     */
    hidePopup(isHide, text) {
      this.isHidePopup = isHide;
      this.popupText = text;
    },

    /**
     * Hiện toast message
     * Author: VQBao - 10/8/2022
     */
    hideToast(text) {
      var me = this;
      me.toastText = text;
      me.isHideToast = false;
      setTimeout(function () {
        me.isHideToast = true;
      }, 3000);
    },

    /**
     * Xóa người dùng khi bấm vào button xóa ở cuối dòng trong bảng
     * Author: VQBao - 10/9/2022
     */
    deleteUserOnClick(e) {
      this.userIdToDelete = e.data.UserID;
      var txtPopup = "Bạn có chắc chắn muốn xóa người dùng "+ e.data.FullName +" không?"
      this.hidePopup(false, txtPopup);
    },

    /**
     * Xóa người dùng(dùng để emit sau confirm xóa)
     * Author: VQBao - 10/9/2022
     */
    deleteUser() {
      document.getElementById("loader").style.display = "block";
      try {
        axios
          .delete(`https://localhost:44382/api/v1/Users/` + this.userIdToDelete)
          .then(() => {
            // load lại data
            this.getUserData({
              pageSize: this.userPaging.pageSize,
              pageNumber: 1,
              searchByText: this.textSearch,
              searchByGroup: this.groupItemSearch,
            });
            document.getElementById("loader").style.display = "none";
            this.hideToast("Xóa người dùng thành công");
          });
      } catch (error) {
        document.getElementById("loader").style.display = "none";
        console.log(error);
      }
    },

    /**
     * Ẩn hiện cửa sổ popup tùy chỉnh cột
     * Author: VQBao - 30/8/2022
     */
    reorderColumn() {
      this.isHideReorderPopup = !this.isHideReorderPopup;
    },

    /**
     * Xuất khẩu toàn bộ người dùng ra file excel
     * Author: VQBao - 12/9/2022
     */
    exportToExcel() {
          document.getElementById("loader").style.display = "block";

      try {
        axios({
          url: "https://localhost:44382/api/v1/Users/ExportToExcel",
          method: "GET",
          responseType: "blob",
        }).then((response) => {
          var fileURL = window.URL.createObjectURL(new Blob([response.data]));
          var fileLink = document.createElement("a");

          fileLink.href = fileURL;
          fileLink.setAttribute("download", "Danh_sach_nguoi_dung.xlsx");
          document.body.appendChild(fileLink);
          fileLink.click();
          document.getElementById("loader").style.display = "none";
        });
      } catch (error) {
        document.getElementById("loader").style.display = "none";
        console.log(error);
      }
    },

    /**
     * Ẩn hiện popup nhập khẩu
     * Author: VQBao - 15/9/2022
     */
    hideDialogImport(isHide){
      this.isHideDialogImport = isHide;
    },

  },
  
};
</script>
<style scoped>
.hide {
  display: none !important;
}
.content-header {
  width: 100%;
  height: 52px;
  padding-bottom: 16px;
  position: relative;
}
.header-text {
  min-width: 110px;
  font-weight: 700;
  font-size: 20px;
}
.header-toolbar {
  width: 100%;
  height: 36px;
  justify-content: flex-end;
}
.toolbar-input {
  width: 305px;
  margin-left: 8px;
  box-sizing: border-box;
  position: relative;
  align-items: center;
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
.toolbar-setting {
  height: 36px;
  width: 36px;
  margin-left: 8px;
  border-radius: 4px;
  border: 1px solid #e0e5e6;
  justify-content: center;
  cursor: pointer;
  position: relative;
}
.popup-resize-col {
  position: absolute;
  top: 50%;
  right: 0;
}
.arrow {
  position: absolute;
  background: #fff;
  height: 14px;
  width: 14px;
  transform: rotate(45deg);
  top: 12px;
  right: 10px;
  z-index: 201;
}
.popup-resize-content {
  position: absolute;
  background-color: #fff;
  top: 22px;
  right: -20px;
  width: 330px;
  height: auto;
  max-height: 798px;
  min-height: 408px;
  padding: 8px 0 0 0;
  box-sizing: border-box;
  border: 1px solid #fff;
  box-shadow: 0 4px 16px rgb(23 27 42 / 24%);
  color: var(--text-primary-color);
  border-radius: 8px;
  z-index: 200;
}
.content-grid {
  height: calc(100% - 108px);
  width: 100%;
  box-sizing: border-box;
}
.header-title {
  font-size: 18px;
  font-weight: 700;
}
.custom-column-status {
  color: rgb(102, 209, 129);
  background-color: rgb(232, 248, 236);
  height: 30px;
  padding: 0 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 25px;
}
.red {
  color: rgb(246, 60, 60);;
  background-color: rgb(255, 236, 230);
}
.cell-button {
  justify-content: center;
  display: none;
}
.resize-content-header {
  width: 100%;
  height: 48px;
  position: relative;
  padding: 16px 16px 0 16px;
}
.header-icon {
  position: absolute;
  justify-content: center;
  width: 36px;
  height: 36px;
  right: 10px;
  border-radius: 50%;
}
.header-icon:hover {
  background-color: rgb(220, 220, 220);
}
.header-input {
  width: 100%;
  padding: 12px 16px 8px 16px;
  box-sizing: border-box;
}
.col-block {
  margin-top: 12px;
  width: 100%;
  height: auto;
  padding: 0 16px;
  box-sizing: border-box;
}
.popup-footer {
  widows: 100%;
  height: 61px;
  border-radius: 0 0 4px 4px;
  border-top: 1px solid #dadce3;
  justify-content: flex-end;
  background: #f2f2f2;
  padding: 12px 24px;
  border-radius: 0 0 8px 8px;
}
.setting {
  justify-content: center;
  min-width: 36px;
}
</style>