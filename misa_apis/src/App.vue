<template>
  <div class="container">
    <BaseLoader id="loader" />
    <div class="top-container">
      <TheHeader />
    </div>
    <div class="bottom-container">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import TheHeader from "../src/components/layout/TheHeader.vue";


export default {
  name: "App",
  components: {
    TheHeader,
  },
};
</script>

<style>
@import url(./style/main.css);
</style>
