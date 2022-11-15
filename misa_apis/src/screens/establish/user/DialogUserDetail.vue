<template>
  <div class="dialog-user-detail" :class="{'show': isShow}">
    <div class="icon-close-detail flex" @click="hideDialog"><div class="icon-close"></div></div>
    <div class="top-detail flex">
      <div class="top-left-detail">
        <div class="user-avatar icon-center avatar-girl"></div>
      </div>
      <div class="top-right-detail">
        <div class="user-name">{{userSelected.FullName}}</div>
        <div class="user-email">{{userSelected.Email}}</div>
        <div class="user-status">
          <div class="flex" v-if="userSelected.Status == '1'"><div class="active-dot"></div><div class="active-text">{{UserProperty.StatusActive}}</div></div>
          <div class="flex" v-else><div class="inactive-dot"></div><div class="inactive-text">{{UserProperty.StatusInactive}}</div></div>
        </div>
        <div @click="editUserGroup">
          <BaseButton 
            buttonType="white"
            buttonName="Chỉnh sửa nhóm"
          />
        </div>
      </div>
    </div>
    <div class="middle-detail">
      <div class="middle-detail-text">THÔNG TIN CÔNG VIỆC</div>
      <div class="middle-detail-item flex">
        <div class="item-lable flex">{{UserProperty.JobPosition}}</div>
        <div class="item-content flex">{{userSelected.JobPositionName}}</div>
      </div>
      <div class="middle-detail-item flex">
        <div class="item-lable flex">{{UserProperty.OrganizationUnit}}</div>
        <div v-if="userSelected.OrganizationUnitName != null" class="item-content flex">{{userSelected.OrganizationUnitName}}</div>
        <div v-else class="item-content flex">-</div>
      </div>
      <div class="middle-detail-item flex">
        <div class="item-lable flex">{{UserProperty.DetailOrganization}}</div>
        <div class="item-content flex">{{userSelected.OrganizationName}}</div>
      </div>
      <div class="middle-detail-item flex">
        <div class="item-lable flex">{{UserProperty.Mobile}}</div>
        <div v-if="userSelected.Mobile != null" class="item-content flex">{{userSelected.Mobile}}</div>
        <div v-else class="item-content flex">-</div>
      </div>
      <div class="middle-detail-item flex">
        <div class="item-lable flex" title="Số điện thoại cơ quan">{{UserProperty.WorkPhone}}</div>
        <div v-if="userSelected.WorkPhone != null" class="item-content flex">{{userSelected.WorkPhone}}</div>
        <div v-else class="item-content flex">-</div>
      </div>
    </div>
    <div class="group-detail middle-detail">
      <div class="middle-detail-text">THUỘC NHÓM</div>
      <div class="user-group-text flex">Nhóm người dùng</div>
      <div class="scroll-group-item">
        <div v-for="group in groupOfUserSelected" :key="group.UserGroupID" class="user-group-item flex">{{group.UserGroupName}}</div>
      </div>
    </div>
  </div>
  <PopupEditUserGroup 
    :isHide="isHidePopup"
    @hidePopup="hidePopup"
    :userSelectedId="userSelectedId"
    @hideDialog="hideDialog"
    @loadData="loadData"
    @hideToast="hideToast"
  />
</template>

<script>
import {mapMutations, mapActions, mapGetters} from 'vuex'
import PopupEditUserGroup from "./PopupEditUserGroup.vue";
import Resource from '../../../constants/Resource';
export default {
  components: {
    PopupEditUserGroup,
  },
  props: ['isShow', 'userSelectedId'],
  data() {
    return {
      isHidePopup: true,
    }
  },
  created(){
    this.UserProperty = Resource.UserProperty;
  },
  computed: {
    ...mapGetters(['userSelected', 'groupOfUserSelected'])
  },
  watch: {
    userSelectedId: function(value){
      if(value != null) {
        this.getUserDataById(value);
      }
    }
  },
  methods: {
    ...mapMutations(['getUserById']),
    ...mapActions(['getUserDataById']),
    /**
     * Khi click vào dấu x, tắt form thông tin chi tiết người dùng
     * Author: VQBao - 30/8/2022
     */
    hideDialog(){
      this.$emit("showDialog", false);
    },
    /**
     * Khi bấm vào button chỉnh sửa nhóm thì hiện popup chỉnh sửa nhóm
     * Author: VQBao - 30/8/2022
     */
    editUserGroup(){
      this.hidePopup(false);
    },
    /**
     * Ẩn hiện popup chỉnh sửa nhóm
     * Author: VQBao - 30/8/2022
     */
    hidePopup(isHide){
      this.isHidePopup = isHide;
    },
    /**
     * Hàm load lại phân trang dữ liệu người dùng
     * Author: VQBao - 10/9/2022
     */
    loadData(){
      this.$emit("loadData")
    },
    /**
     * Hiển thị toast message
     * Author: VQBao - 10/9/2022
     */
    hideToast(text){
      this.$emit("hideToast", text);
    }
  },
};
</script>

<style scoped>
.show {
  display: block !important;
}
.dialog-user-detail {
  display: none;
  width: 40%;
  min-width: 500px;
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
.top-detail {
  width: 100%;
  height: 120px;
}
.top-left-detail {
  width: 96px;
  height: 100%;
  box-sizing: border-box;
  padding-top: 10px;
}
.user-avatar {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  box-sizing: border-box;
  background-size: cover;
}
.top-right-detail {
  width: max-content;
  height: 100%;
}
.user-name {
  line-height: 24px;
  font-size: 20px;
}
.user-email {
  line-height: 20px;
}
.user-status {
  height: 35px;
}
.active-dot {
  height: 8px;
  width: 8px;
  border-radius: 50%;
  background-color: rgb(102, 209, 129);
  margin-right: 8px;
  
}
.active-text{
  color: rgb(102, 209, 129);
}
.inactive-dot {
  height: 8px;
  width: 8px;
  border-radius: 50%;
  background-color: rgb(246, 60, 60);
  margin-right: 8px;
}
.inactive-text{
  color: rgb(246, 60, 60);
}
.middle-detail {
  margin-top: 20px;
}
.middle-detail-text {
  font-size: 16px;
}
.middle-detail-item {
  margin-top: 16px;
}
.item-lable {
  height: 35px;
  width: 165px;
}
.item-content {
  width: 285px;
  height: 36px;
  border-bottom: 1px solid rgb(224, 229, 230);
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
  padding-right: 4px;
}
.user-group-text {
  margin-top: 20px;
  font-weight: 700;
  width: 100%;
  height: 47.5px;
  padding-left: 16px;
  border-bottom: 1px solid rgb(224, 229, 230);
  cursor: pointer;
}
.user-group-item {
  width: 100%;
  height: 47.5px;
  padding-left: 16px;
  border-bottom: 1px solid rgb(224, 229, 230);
  cursor: pointer;
}
.user-group-item:hover {
  background-color: var(--primary-bg);
}
.scroll-group-item {
    height: 180px;
    box-sizing: border-box;
    overflow: auto;
}
</style>