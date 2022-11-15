<template>
<div class="header flex">
    <div class="header-left flex">
        <div class="button-sidebar icon-center icon-sidebar"></div>
        <div class="header-logo icon-center logo"></div>
        <div class="header-navbar flex">
            <router-link to="/schedule" class="navbar-item flex" style="width: 100px">
                <div class="navbar-item-text">{{TheHeader.Schedule}}</div>
            </router-link>
            <router-link to="/mission" class="navbar-item flex">{{TheHeader.Mission}}</router-link>
            <router-link to="/transport" class="navbar-item flex" style="width: 115px">{{TheHeader.Transport}}</router-link>
            <router-link to="/advance" class="navbar-item flex">{{TheHeader.Advance}}</router-link>
            <router-link to="/pay" class="navbar-item flex">{{TheHeader.Pay}}</router-link>
            <router-link to="/establish/user" class="navbar-item flex" :class="[{'router-link-active active': $route.path.indexOf('/establish/') > -1}]">{{TheHeader.Establish}}</router-link>
        </div>
    </div>
    <div class="header-right">
        <div class="header-right-content flex">
            <div class="header-icon icon-noti icon-center"></div>
            <div class="header-icon icon-help icon-center"></div>
            <div class="header-icon header-avatar avatar-girl icon-center"></div>
        </div>
    </div>
</div>
</template>
<script>
import Resource from '../../constants/Resource'
export default {
    created(){
        this.TheHeader = Resource.TheHeader;
    }
}
</script>
<style scoped>
.header {
    width: 100%;
    height: 100%;
    box-shadow: 0 4px 2px -2px rgb(233, 232, 232);
    box-sizing: border-box;
}
.header-left {
    height: 100%;
}
.button-sidebar {
    width: 36px;
    height: 36px;
    margin: 10px 16px;
    cursor: pointer;
}
.button-sidebar:hover {
    border-radius: 50%;
    background-color: #f2f2f2;
}
.header-logo {
    width: 175px;
    height: 34px;
    margin-right: 5px;
    cursor: pointer;
}
.header-navbar{
    height: 56px;
    margin-left: 34px;
    width: max-content;

}
.navbar-item{
    height: 56px;
    width: 80px;
    justify-content: center;
    cursor: pointer;
    border-bottom: 4px solid;
    border-color: #fff;
    display: flex;
    align-items: center;
}
.navbar-item:hover {
    color: rgb(39, 183, 248) !important;
    border-bottom: 4px solid;
    border-color: rgb(39, 183, 248);
}
.header-navbar .router-link-active {
    color: rgb(39, 183, 248) !important;
    border-bottom: 4px solid;
    border-color: rgb(39, 183, 248);
}

.header-right {
    width: calc(100% - 817px);
    height: 100%;
    position: relative;
}
.header-right-content {
    width: 160px;
    height: 100%;
    position: absolute;
    right: 16px;
}
.header-icon {
    width: 36px;
    height: 36px;
    margin-right: 20px;
    border-radius: 50%;
    cursor: pointer;
}
.header-avatar {
    margin-right: 0px;
    margin-left: 6px;
}
.header-icon:hover {
    background-color: #f2f2f2;
}
</style>