<template>
  <div class="m-paging flex">
    <div class="paging-left flex">Tổng số:&nbsp;<strong>{{totalRecords}}</strong>&nbsp;bản ghi</div>
    <div class="paging-right flex">
      <span class="page-size-text">Số bản ghi/trang</span>
      <div class="page-size-box">
        <DxSelectBox
          placeholder=""
          :data-source="pageSelection"
          v-model:value="dPageSize"
        />
      </div>
      <div class="page-size-range"><strong>{{recordStart}}</strong>-<strong>{{recordEnd}}</strong>&nbsp;bản ghi</div>
      <div class="page-nav-button flex" @click="toPrevPage"><div class="icon-prev"></div></div>
      <div class="page-nav-button flex" @click="toNextPage"><div class="icon-next"></div></div>
    </div>
  </div>
</template>

<script>
import DxSelectBox from "devextreme-vue/select-box";
export default {
  components: {DxSelectBox},
  props: ['totalRecords', 'totalPages', 'pageSize', 'pageNumber','recordStart', 'recordEnd'],
  data() {
    return {
      pageSelection: [15, 25, 50, 100],
      dPageSize: 50,
    }
  },
  watch: {
    pageSize: function(value){
      this.dPageSize = value;
    },
    dPageSize: function(value){
      this.$emit("getData", value, 1)
    }
  },
  methods: {
    /**
     * Chuyển về trang trước đó
     * Author: VQBao - 8/9/2022
     */
    toPrevPage(){
      if(this.pageNumber > 1){
        this.$emit("getData", this.dPageSize, this.pageNumber-1)
      }
    },
    
    /**
     * Chuyển đến trang tiếp theo
     * Author: VQBao - 8/9/2022
     */
    toNextPage(){
      if(this.pageNumber < this.totalPages){
        this.$emit("getData", this.dPageSize, this.pageNumber+1)
      }
    }
  }
};
</script>

<style scoped>
.m-paging {
  height: 56px;
  width: 100%;
  border: none;
  padding: 10px 0 !important;
  z-index: 8;
  position: relative;
  background-color: #f1f5f9;
  border-radius: 8px;
}
.paging-left {
  position: absolute;
  left: 16px;
}
.paging-right {
  position: absolute;
  right: 16px;
}
.page-size-text {
  margin-right: 8px;
}
.page-size-box {
  width: 80px;
  position: relative;
  box-sizing: border-box;
}

.page-size-range {
    padding: 0 24px;
}
.page-nav-button {
    width: 36px;
    height: 36px;
    justify-content: center;
    cursor: pointer;
}

</style>