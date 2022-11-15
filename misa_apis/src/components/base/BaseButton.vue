<template>
  <button class="m-button m-button-blue" v-if="buttonType == 'regular'">{{buttonName}}</button>
  <button class="m-button m-button-white" v-if="buttonType == 'white'">{{buttonName}}</button>
  <button class="m-button m-button-red" v-if="buttonType == 'red'">{{buttonName}}</button>
  <button class="m-button m-button-blue flex" v-if="buttonType == 'icon'">
    <div class="button-icon icon-add"></div>
    {{buttonName}}
  </button>
</template>

<script>
export default {
    props: ['buttonType','buttonName']
}
</script>

<style scoped>
.m-button {
    min-width: 98px;
    min-height: 36px;
    border: 1px solid var(--button-primary-normal-border-color);
    border-radius: 4px;
    background-color: var(--button-primary-normal-bg-color);
    box-sizing: border-box;
    padding: 0 16px 0 12px;
    color: var(--text-white-primary-color);
    cursor: pointer;
    margin-left: 8px;
}
.m-button-blue:hover {
    background-color: var(--button-primary-hover-bg-color);
    border-color: var(--button-primary-hover-border-color);
    color: var(--text-white-secondary-color);
}
.button-icon {
    justify-content: center;
    padding-left: 4px;
}
.m-button-white {
  border-color: #e0e5e6;
  background-color: #fff;
  color: var(--text-primary-color);
  margin-left: 0px;
}
.m-button-white:hover {
  background-color: var(--primary-bg);
}
.m-button-red {
  border-color: rgb(255, 153, 0);
  background-color: rgb(255, 255, 255);
  color: rgb(255, 99, 51);
}
.m-button-red:hover {
  background-color: rgb(255, 255, 255);
    color: rgb(255, 153, 0);
    border: 1px solid rgb(255, 153, 0);
}
</style>