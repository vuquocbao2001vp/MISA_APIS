<template>
<div class="menu flex">
    <router-link to="/establish/user" class="menu-item flex">
        <div class="item-icon flex"><div class="icon-user"></div></div>
        <div class="item-text flex">{{TheMenu.User}}</div>
    </router-link>
    <router-link to="/establish/user-group" class="menu-item flex">
        <div class="item-icon flex"><div class="icon-user-group"></div></div>
        <div class="item-text flex">{{TheMenu.UserGroup}}</div>
    </router-link>
    <router-link to="/establish/authority" class="menu-item flex">
        <div class="item-icon icon-center icon-authori"></div>
        <div class="item-text flex">{{TheMenu.Authority}}</div>
    </router-link>
    <router-link to="/establish/digital-sign" class="menu-item flex">
        <div class="item-icon icon-center icon-sign"></div>
        <div class="item-text flex">{{TheMenu.DigitalSign}}</div>
    </router-link>
</div>
</template>
<script>
import Resource from '../../constants/Resource'
export default {
    created(){
        this.TheMenu = Resource.TheMenu;
    }
}
</script>
<style scoped>
.menu {
    width: 200px;
    height: 100%;
    box-sizing: border-box;
    background-color: var(--primary-bg);
    padding: 16px 12px 0px 16px;
    flex-direction: column;
}
.menu-item {
    height: 44px;
    width: 172px;
    padding-left: 12px;
    border-radius: 8px;
}
.menu-item:hover {
    background-color: rgb(229, 234, 243);
}
.item-icon {
    width: 24px;
    height: 24px;
    margin: 0 8px 4px 0;
    justify-content: center;
}
.item-text {
    line-height: 44px;
}
.menu .router-link-exact-active {
    background-color: #e2e8f0 !important;
    color: var(--text-primary-color) !important;
    font-weight: 600;
}
.menu .router-link-exact-active .item-icon .icon-user {
    background-color: var(--text-primary-color) !important;
}
.menu .router-link-exact-active .item-icon .icon-user-group {
    background-color: var(--text-primary-color) !important;
}
.menu .router-link-exact-active .icon-authori {
    background-color: var(--text-primary-color) !important;
}
.menu .router-link-exact-active .icon-sign {
    background-color: var(--text-primary-color) !important;
}
</style>