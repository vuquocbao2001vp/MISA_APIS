<template>
  <div class="content-header flex">
    <span class="header-text">Nhóm người dùng</span>
    <div class="header-toolbar flex">
      <div class="toolbar-input flex">
        <div class="icon-textbox"></div>
        <DxTextBox
          class="control-textbox"
          placeholder="Tìm kiếm tên nhóm"
          value-change-event="keyup"
          @value-changed="getTextSearch"
        />
      </div>
    </div>
  </div>
  <div class="content-grid">
    <DxDataGrid
      :data-source="userGroups"
      :allow-column-reordering="true"
      :allow-column-resize="true"
      :hover-state-enabled="true"
      :selection="{ mode: 'single' }"
      @row-click="rowOnClick"
      no-data-text="Không có dữ liệu để hiển thị"
    >
      <DxPaging :enabled="false" />

      <DxColumn data-field="UserGroupName" caption="Tên nhóm" />
      <DxColumn
        data-field="TotalMember"
        caption="Số thành viên"
        alignment="right"
        :width="160"
      />
      <DxColumn data-field="Description" caption="Mô tả" :width="400" />
      <DxColumn
        data-field="Status"
        caption="Trạng thái"
        alignment="center"
        cell-template="cellTemplateStatus"
        :width="154"
      />
      <template #cellTemplateStatus="{ data }">
        <div class="custom-column-status" v-if="data.value == '1'">
          Đang sử dụng
        </div>
        <div class="custom-column-status red" v-else>Không sử dụng</div>
      </template>
    </DxDataGrid>
  </div>
  <BasePaging
    :totalRecords="userGroupPaging.totalRecords"
    :totalPages="userGroupPaging.totalPages"
    :pageSize="userGroupPaging.pageSize"
    :pageNumber="userGroupPaging.pageNumber"
    :recordStart="userGroupPaging.recordStart"
    :recordEnd="userGroupPaging.recordEnd"
    @getData="loadGroupPaging"
  />
  <DialogUserGroupDetail
    :isHide="isHideDialog"
    @hideDialog="hideDialog"
    :groupSelectedId="groupSelectedId"
    @loadData="loadData"
    @hideToast="hideToast"
  />
  <BaseToast
    :isHideToast="isHideToast"
    :text="toastText"
  />
</template>
<script>
import { mapMutations, mapActions, mapGetters } from "vuex";
import DialogUserGroupDetail from "./DialogUserGroupDetail.vue";
export default {
  components: {
    DialogUserGroupDetail,
  },
  data() {
    return {
      isHideDialog: true,
      isHideToast: true,
      toastText: "",
      searchKey: "",
      timeout: null,
      groupSelectedId: "",
    };
  },
  computed: {
    ...mapGetters(["userGroups", "userGroupPaging"]),
  },
  mounted() {
    this.getUserGroupData({
      pageSize: 50,
      pageNumber: 1,
      searchKey: "",
    });
  },
  watch: {
    /**
     * Khi từ khóa tìm kiếm thay đổi, load lại paging
     * Author: VQBao - 8/9/2022
     */
    searchKey: function (value) {
      try {
        this.getUserGroupData({
          pageSize: this.userGroupPaging.pageSize,
          pageNumber: 1,
          searchKey: value,
        });
        this.groupSelectedId = null;
      } catch (error) {
        console.log(error);
      }
    },
  },
  methods: {
    ...mapMutations(["getUserGroupPaging"]),
    ...mapActions(["getUserGroupData"]),
    /**
     * Phương thức phân trang nhóm người dùng
     * Author: VQBao - 8/9/2022
     */
    loadGroupPaging(pageSize, pageNumber) {
      this.getUserGroupData({
        pageSize: pageSize,
        pageNumber: pageNumber,
        searchKey: this.searchKey,
      });
      this.groupSelectedId = null;
    },
    /**
     * Load lại dữ liệu phân trang nhóm người dùng
     * Author: VQBao - 12/9/2022
     */
    loadData(id){
      this.getUserGroupData({
        pageSize: this.userGroupPaging.pageSize,
        pageNumber: this.userGroupPaging.pageNumber,
        searchKey: this.searchKey,
      });
      if(id != null){
        this.groupSelectedId = id
      } else {
        this.groupSelectedId = null;
      }
    },
    /**
     * Lấy ra từ khóa tìm kiếm sau khi nhập xong input
     * Author; VQBao - 8/9/2022
     */
    getTextSearch(data) {
      clearTimeout(this.timeout);
      var self = this;
      self.timeout = setTimeout(function () {
        self.searchKey = data.value;
      }, 500);
    },
    /**
     * Ẩn hiện form thông tin chi tiết nhóm người dùng
     * Author: VQBao - 8/9/2022
     */
    hideDialog(isHide) {
      this.isHideDialog = isHide;
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
     * Khi click vào hàng trong bảng thì cửa sổ thông tin chi tiết hiện ra
     * Author: VQBao - 30/8/2022
     */
    rowOnClick(groupRow) {
      this.groupSelectedId = groupRow.data.UserGroupID;
      this.hideDialog(false);
    },
  },
};
</script>
<style scoped>
.content-header {
  width: 100%;
  height: 52px;
  padding-bottom: 16px;
  position: relative;
}
.header-text {
  min-width: 110px;
  font-size: 20px;
  font-weight: 700;
}
.header-toolbar {
  position: absolute;
  right: 0;
  width: max-content;
  height: 36px;
}
.toolbar-input {
  width: 305px;
  margin-right: 8px;
  box-sizing: border-box;
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
.toolbar-setting {
  height: 36px;
  width: 36px;
  margin-left: 8px;
  border-radius: 4px;
  border: 1px solid #e0e5e6;
  justify-content: center;
  cursor: pointer;
}
.content-grid {
  width: 100%;
  height: calc(100% - 108px);
  box-sizing: border-box;
}
.custom-column-status {
  color: rgb(102, 209, 129);
  background-color: rgb(232, 248, 236);
  height: 30px;
  padding: 0 14px;
  width: 128px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 25px;
}
.red {
  color: rgb(237, 98, 98);
  background-color: rgb(241, 186, 186);
}
.cell-button {
  justify-content: center;
}
</style>