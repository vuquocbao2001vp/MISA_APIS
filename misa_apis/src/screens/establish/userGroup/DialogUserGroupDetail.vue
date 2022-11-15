<template>
  <div class="dialog-user-group-detail" :class="{ hide: isHide }">
    <div class="icon-close-detail flex" @click="hideDialog">
      <div class="icon-close"></div>
    </div>
    <div class="top-block">
      <div class="group-title flex">
        <div class="group-name">{{ groupSelected.UserGroupName }}</div>
        <div class="group-status flex">
          <div class="active-dot"></div>
          Đang sử dụng
        </div>
      </div>
      <div class="group-description">{{ groupSelected.Description }}</div>
      <div class="line-16"></div>
      <div class="group-member-text">
        Thành viên (<span>{{ userOfGroupSelected.length }}</span
        >)
      </div>
      <div class="block-toolbar flex">
        <div class="block-input flex">
          <div class="icon-textbox"></div>
          <DxTextBox
            class="control-textbox"
            placeholder="Tìm kiếm thành viên"
            value-change-event="keyup"
            @value-changed="getTextSearch"
          />
        </div>
        <div class="block-button-add" @click="addNewUser">
          <BaseButton buttonType="icon" buttonName="Thêm thành viên" />
        </div>
      </div>
      <div class="block-navbar flex" v-show="listDeleteUser.length > 0">
        <div class="selected-quantity"><span>{{listDeleteUser.length}}</span> đang chọn</div>
        <div class="disselect-span" @click="uncheckAllUser">Bỏ chọn</div>
        <div class="delete-member-button" @click="deleteUser('','')">
          <BaseButton buttonType="red" buttonName="Loại bỏ thành viên" />
        </div>
      </div>
    </div>
    <div id="scrollBlock" class="scroll-block">
      <BaseLoader id="loaderDialog" />
      <div
        class="member-item flex"
        v-for="(user, index) in userOfGroupSelected"
        :key="user.UserID"
      >
        <div class="member-item-checkbox">
          <div @click="checkUserCheckbox(user.UserID, index)"><DxCheckBox v-model:value="checkUserToDelete[index]"/></div>
        </div>
        <div class="member-image icon-center avatar-girl"></div>
        <div class="member-info">
          <div class="member-name flex">
            {{ user.FullName }} 
            <span v-if="user.Email != null" class="flex">(<div class="member-email">{{ user.Email }}</div>)</span>
          </div>
          <div class="member-position flex">
            {{ user.JobPositionName }}
            <span v-if="user.OrganizationUnitName != null" class="flex">&nbsp;- {{ user.OrganizationUnitName }}</span>
            <span v-if="user.OrganizationName != null" class="flex">&nbsp;- {{ user.OrganizationName }}</span>
          </div>
        </div>
        <div class="item-delete-icon flex" @click="deleteUser(user, index)"><div class="icon-delete"></div></div>
      </div>
    </div>
  </div>
  <PopupAddUser :groupSelected="groupSelected" :isHide="isHidePopup" @hidePopup="hidePopup" @hideToast="hideToast" :userSelected="userOfGroupSelected"/>
  <BasePopup
    :isHidePopup="isHidePopupDelete"
    title="Thông báo"
    :text="popupDeleteText"
    @confirmPopupAction="deleteMultiUserOfGroup"
    @hidePopup="hidePopupDelete"
  />
</template>
<script>
import axios from "axios";
import BaseApi from '../../../constants/BaseApi'
import { mapMutations, mapActions, mapGetters } from "vuex";
import PopupAddUser from "./PopupAddUser.vue";
import Enum from '../../../constants/Enum'
export default {
  components: {
    PopupAddUser,
  },
  props: ["isHide", "groupSelectedId"],
  data() {
    return {
      isHidePopup: true,
      isHidePopupDelete: true,
      popupDeleteText: "",
      timeout: null,
      textSearch: "",
      checkUserToDelete: [],
      listDeleteUser: [],
      userOfGroupSelected: [],
      deleteMode: null,
      userDeleteId: null,
    };
  },
  computed: {
    ...mapGetters(["groupSelected"]),
  },
  watch: {
    groupSelectedId: function (value) {
      if (value != null) {
        this.getGroupDataById(value);
        this.getUserDataOfGroupPaging(100, 1, value);
        this.listDeleteUser = [];
        this.checkUserToDelete = [];
      }
    },
    textSearch: function (value) {
      this.getUserDataOfGroupPaging(100, 1, this.groupSelectedId, value);
    },
    isHidePopup: function(value){
      if(value){
        this.getUserDataOfGroupPaging(100, 1, this.groupSelectedId);
      }
    },
    isHide: function(value) {
      if(value){
        this.listDeleteUser = [];
        this.checkUserToDelete = [];
      }
    }
  },
  methods: {
    ...mapMutations(["getGroupById"]),
    ...mapActions(["getGroupDataById"]),
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
     * Ẩn dialog khi bấm vào icon tắt
     * Author: VQBao - 30/8/2022
     */
    hideDialog() {
      this.$emit("hideDialog", true);
    },
    /**
     * Khi click vào button thêm thành viên thì mở popup thêm thành viên
     * Author: VQBao - 30/8/2022
     */
    addNewUser() {
      this.hidePopup(false);
    },
    /**
     * Ẩn hiện popup thêm thành viên
     * Author: VQBao - 30/8/2022
     */
    hidePopup(isHide) {
      this.isHidePopup = isHide;
    },
    /**
     * Ẩn hiện popup thông báo
     * Author: VQBao - 30/8/2022
     */
    hidePopupDelete(isHide, text){
      this.isHidePopupDelete = isHide;
      this.popupDeleteText = text;
    },
    /**
     * Khi click xóa, hiện popup cảnh báo
     * Author: VQBao - 30/8/2022
     */
    deleteUser(user, index){
      if(user == '' && index == ''){
        this.deleteMode = Enum.DeleteMode.Multi;
        this.hidePopupDelete(false, "Bạn có chắc chắn muốn xóa những người dùng đang chọn không?")
      } else {
        this.deleteMode = Enum.DeleteMode.Single;
        this.userDeleteId = user.UserID;
        this.hidePopupDelete(false, "Bạn có chắc chắn muốn xóa người dùng "+ user.FullName +" không?")
      }
    },
    /**
     * Thực hiện xóa nhiều người dùng ra khỏi nhóm
     * Author: VQBao - 12/9/2022
     */
    async deleteMultiUserOfGroup(){
      // Xóa nhiều người dùng khi bấm "Loại bỏ thành viên"
      if(this.deleteMode == Enum.DeleteMode.Multi){
        if(this.listDeleteUser.length < this.userOfGroupSelected.length){
          try {
            await axios.delete(BaseApi.UserGroupApi+`/DeleteMultiUserOfGroup?groupId=` + this.groupSelectedId,{ data: this.listDeleteUser}).then(() => {
              this.checkUserToDelete = [];
              this.listDeleteUser = [];
              this.hideDialog();
              this.$emit("loadData");
              this.$emit("hideToast", "Xóa người dùng thành công")
            })
          } catch (error) {
            console.log(error);
          }
        }
      } else {
        // Xóa người dùng khi click vào icon xóa
        try {
          var listId = [];
          listId.push(this.userDeleteId);
          document.getElementById("loaderDialog").style.display = "block";
          await axios.delete(BaseApi.UserGroupApi+`/DeleteMultiUserOfGroup?groupId=` + this.groupSelectedId,{ data: listId}).then(() => {
              let id = this.listDeleteUser.indexOf(this.userDeleteId);
              this.listDeleteUser.splice(id, 1);
              this.getUserDataOfGroupPaging(100, 1, this.groupSelectedId);
              this.$emit("loadData", this.groupSelectedId);
              this.$emit("hideToast", "Xóa người dùng thành công")
              document.getElementById("loaderDialog").style.display = "none";
            })
        } catch (error) {
          document.getElementById("loaderDialog").style.display = "none";
          console.log(error);
        }
      }
    },
    /**
     * Lấy số lượng checkbox đang được check
     * Author: VQBao - 12/9/2022
     */
    checkUserCheckbox(userId, index){
      if(this.checkUserToDelete[index]){
        this.listDeleteUser.push(userId);
      } else {
        let id = this.listDeleteUser.indexOf(userId);
        this.listDeleteUser.splice(id, 1);
      }
      if(this.listDeleteUser.length > 0){
        document.getElementById("scrollBlock").style.height = "calc(100% - 278px)";
      } else {
        document.getElementById("scrollBlock").style.height = "calc(100% - 226px)";
      }
    },
    /**
     * Bỏ chọn tất cả các checkbox
     * Author: VQBao - 12/9/2022
     */
    uncheckAllUser(){
      this.checkUserToDelete = [];
      this.listDeleteUser = [];
    },
    /**
     * Hiện toast thông báo
     * Author: VQBao - 12/9/2022
     */
    hideToast(text){
      this.$emit("hideToast", text)
    },
    /**
   * Phân trang tìm kiếm thành viên thuộc nhóm người dùng
   * Author: VQBao - 8/9/2022
   */
    async getUserDataOfGroupPaging(pageSize, pageNumber, userGroupId, searchByText) {
      document.getElementById("loaderDialog").style.display = "block";
      try {
        await axios
          .get(BaseApi.UserGroupApi+`/MemberOfGroupPaging`, {
            params: {
              pageSize: pageSize,
              pageNumber: pageNumber,
              userGroupId: userGroupId,
              searchByText: searchByText
            },
          })
          .then((response) => {
            this.userOfGroupSelected = response.data.Data;
            this.$emit("loadData", this.groupSelectedId);
            document.getElementById("loaderDialog").style.display = "none";
          });
      } catch (error) {
        document.getElementById("loaderDialog").style.display = "none";
        console.log(error);
      }
    },
  }
}
</script>

<style scoped>
.hide {
  display: none !important;
}
.dialog-user-group-detail {
  width: 663px;
  height: 100vh;
  box-shadow: 0 0 10px 0 rgb(0 0 0 / 10%);
  z-index: 8;
  background-color: #fff;
  position: absolute;
  top: 0;
  right: 0;
  overflow: auto;
  padding: 24px;
  box-sizing: border-box;
}
.icon-close-detail {
  position: absolute;
  top: 24px;
  right: 24px;
  justify-content: center;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  cursor: pointer;
}
.icon-close-detail:hover {
  background-color: rgb(220, 220, 220);
}
.group-name {
  font-size: 20px;
  font-weight: 700;
}
.group-title {
  height: 24px;
  box-sizing: border-box;
}
.group-status {
  color: rgb(102, 209, 129);
  font-weight: 700;
  height: 24px;
  box-sizing: border-box;
}
.active-dot {
  height: 8px;
  width: 8px;
  border-radius: 50%;
  background-color: rgb(102, 209, 129);
  margin-right: 4px;
  margin-left: 4px;
}
.group-description {
  margin-top: 16px !important;
}
.line-16 {
  margin-top: 40px;
  border-bottom: 1px solid #ccc;
}
.group-member-text {
  margin-top: 24px;
  margin-bottom: 12px;
  font-weight: 700;
}
.block-toolbar {
  height: 36px;
  position: relative;
  margin-bottom: 16px;
}
.block-input {
  width: 288px;
  position: relative;
  box-sizing: border-box;
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
  width: 100% !important;
  padding-left: 28px;
}
.block-button-add {
  position: absolute;
  right: 0;
}
.block-navbar {
  height: 36px;
  margin-bottom: 16px;
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
.scroll-block {
  position: relative;
  height: calc(100% - 226px);
  width: 100%;
  box-sizing: border-box;
  overflow: auto;
}
.member-item {
  height: 64px;
  width: 100%;
  position: relative;
}
.member-item:hover {
  background-color: var(--primary-bg);
}
.member-item-checkbox {
  height: 18px;
  width: 18px;
  box-sizing: border-box;
  margin-right: 12px;
}
.member-image {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  margin-right: 12px;
}
.member-name {
  color: #2c3e50;
}
.member-email {
  color: #616767;
}
.member-position {
  color: #616767;
}
.item-delete-icon {
  position: absolute;
  width: 24px;
  height: 24px;
  justify-content: center;
  right: 16px;
  cursor: pointer;
}
</style>