<template>
<div class="content">
    <router-view></router-view>
</div>
</template>
<script>
export default {
    
}
</script>
<style scoped>
.content {
    width: calc(100% - 200px);
    height: 100%;
    box-sizing: border-box;
    padding: 16px 20px;
} 
</style>