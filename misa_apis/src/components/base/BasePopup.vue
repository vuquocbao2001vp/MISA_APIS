<template>
  <div class="popup flex" :class="{ 'hide-popup': isHidePopup }">
    <div class="popop-content">
      <div class="popup-close icon-center icon-close"></div>
      <span class="popup-title">{{ title }}</span>
      <span class="popup-text">{{ text }}</span>
      <div class="popup-button flex">
        <div @click="closePopup"><BaseButton buttonType="white" buttonName="Đóng" /></div>
        <div @click="confirmPopup"><BaseButton buttonType="regular" buttonName="Đồng ý" /></div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  props: ["isHidePopup", "title", "text"],
  methods: {
    /**
     * Khi ấn vào button đóng thì đóng popup
     * Author: VQBao - 10/9/2022
     */
    closePopup(){
        this.$emit("hidePopup", true)
    },
    
    /**
     * Khi ấn vào button đồng ý thì thực hiện action đồng ý
     * Author: VQBao - 10/9/2022
     */
    confirmPopup(){
        this.$emit("confirmPopupAction");
        this.$emit("hidePopup", true)
    }
  },
};
</script>
<style scoped>
.hide-popup {
  display: none !important;
}
.popup {
  position: fixed;
  align-items: center;
  justify-content: center;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.24);
  z-index: 100;
}

.popop-content {
  width: 400px;
  background-color: #ffffff;
  position: relative;
  display: flex;
  flex-direction: column;
}

.popup-close {
  width: 40px;
  height: 40px;
  position: absolute;
  top: 0;
  right: 0;
  cursor: pointer;
}

.popup-title {
  font-size: 24px;
  margin: 20px 24px 12px 24px;
}

.popup-text {
  margin: 0 24px 16px 24px;
  line-height: 24px;
}

.popup-button {
  justify-content: flex-end;
  margin-right: 24px;
  margin-bottom: 24px;
}

.popup-button button {
  margin-right: 8px;
}
</style>