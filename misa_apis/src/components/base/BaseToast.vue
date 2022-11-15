<template>
  <div class="toast-message flex" :class="{'hide-toast': isHideToast}">
    <div class="toast-icon flex"><div class="icon-toast-success"></div></div>
    <div class="toast-text">{{text}}</div>
  </div>
</template>
<script>
export default {
    props: ['isHideToast', 'text']
};
</script>
<style scoped>
.hide-toast {
  display: none !important;
}

.toast-message {
  z-index: 300;
  position: fixed;
  top: 120px;
  left: 560px;
  height: 40px;
  display: flex;
  align-items: center;
  border-radius: 4px;
  border: 1px solid var(--input-normal-border-color);
  box-sizing: border-box;
  background-color: var(--primary-bg);
  padding: 0 8px;
}

.toast-icon {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background-color: var(--primary);
  justify-content: center;
}

.toast-text {
  margin-left: 8px;
  width: max-content;
  font-size: 16px;
}

</style>