<template>
  <div class="popup" :class="{ hide: isHide }">
    <div class="popup-form">
      <div class="popup-header">
        <h1 class="popup-header-title">Chỉnh sửa nhóm</h1>
        <div class="popup-header-close flex" @click="closePopup">
          <div class="icon-close"></div>
        </div>
      </div>
      <div class="popup-content">
        <div class="user-detail flex">
          <div class="user-avatar icon-center avatar-girl"></div>
          <div class="user-info">
            <div class="user-name flex">
              {{ userSelected.FullName }}
              <div class="user-email">({{ userSelected.Email }})</div>
            </div>
            <div class="user-position">{{ userSelected.OrganizationName }}</div>
          </div>
        </div>
        <div class="user-group-text">
          Nhóm người dùng<span class="require-text">*</span>
        </div>
        <div
          class="group-item flex"
          v-for="(group, index) in userGroups"
          :key="group.UserGroupID"
        >
          <DxCheckBox v-model="checkGroup[index]" :text="group.UserGroupName" />
        </div>
      </div>
      <div class="popup-footer flex">
        <div class="popup-footer-button flex">
          <div @click="closePopup">
            <BaseButton buttonType="white" buttonName="Hủy bỏ" />
          </div>
          <div @click="editGroupOfUser">
            <BaseButton buttonType="regular" buttonName="Lưu" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mapMutations, mapActions, mapGetters } from "vuex";
export default {
  props: ["isHide", "userSelectedId"],
  data() {
    return {
      checkGroup: [],
      listGroupId: [],
    };
  },
  computed: {
    ...mapGetters(["userSelected", "groupOfUserSelected", "userGroups"]),
  },
  mounted() {
    this.getAllGroupData();
  },
  watch: {
    isHide: function (value) {
      if (value == false) {
        for (let m = 0; m < this.groupOfUserSelected.length; m++) {
          for (let index = 0; index < this.userGroups.length; index++) {
            if (
              this.groupOfUserSelected[m].UserGroupName ==
              this.userGroups[index].UserGroupName
            ) {
              this.checkGroup[index] = true;
            }
          }
        }
      } else {
        this.checkGroup = [];
      }
    },
  },
  methods: {
    ...mapMutations(["getUserById", "getAllGroup"]),
    ...mapActions(["getUserDataById", "getAllGroupData"]),
    /**
     * Tắt popup khi click vào icon tắt
     * Author: VQBao - 30/8/2022
     */
    closePopup() {
      this.$emit("hidePopup", true);
    },
    /**
     * Chỉnh sửa nhóm người dùng khi thực hiện click vào lưu ở popup
     * Author: VQbao - 10/9/2022
     */
    async editGroupOfUser() {
      for (let i = 0; i < this.checkGroup.length; i++) {
        if (this.checkGroup[i] == true) {
          this.listGroupId.push(i + 1);
        }
      }
      try {
        await axios.put(`https://localhost:44382/api/v1/Users/UpdateGroupOfUser`,this.listGroupId,{
              params: {
                userId: this.userSelectedId,
              },
              headers: {
                "Content-Type": "application/json",
              },
            }
          )
          .then(() => {
            this.checkGroup = [];
            this.listGroupId = [];
            this.$emit("hidePopup", true);
            this.$emit("hideDialog");
            this.$emit("hideToast", "Chỉnh sửa nhóm của người dùng thành công");
            this.$emit("loadData");
          });
      } catch (error) {
        console.log(error);
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
  width: 496px;
  min-width: 500px;
  height: 550px;
  border-radius: 6px;
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
.popup-content {
  overflow: auto;
  transition: all 0.23s ease 0.1s;
  padding: 24px;
  box-sizing: border-box;
  width: 100%;
  height: 447px;
}
.user-detail {
  height: 64px;
  width: 100%;
}
.user-avatar {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  box-sizing: border-box;
  background-size: cover;
}
.user-info {
  margin-left: 12px;
}
.user-name {
  font-weight: 700;
}
.user-email {
  padding-left: 2px;
  font-weight: 400;
}
.user-group-text {
  font-size: 16px;
  font-weight: 700;
  margin: 16px 0;
}
.require-text {
  color: #e54848;
  padding-left: 2px;
}
.group-item {
  height: 32px;
  width: 100%;
}
.popup-footer {
  height: 50px;
  border-radius: 0 0 4px 4px;
  border-top: 1px solid #dadce3;
  justify-content: flex-end;
  background: #f2f2f2;
  padding: 12px 24px;
  border-radius: 0 0 8px 8px;
}
.popup-footer-button {
  margin-right: 12px;
}
</style>