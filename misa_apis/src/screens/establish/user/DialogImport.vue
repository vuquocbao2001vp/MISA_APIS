<template>
  <div class="dilog-import flex" :class="{ hide: isHideDialogImport }">
    <div class="dialog-import-content">
      <span class="popup-title">Nhập khẩu</span>
      <div class="select-file flex">
        <div class="file-block flex">
          <div class="file-icon icon-import" @click="importFileOnClick"></div>
          <span class="file-title" @click="importFileOnClick">Click để chọn tệp nhập khẩu</span>
          <div @click="exportSampleExcel" class="file-sample flex">Tải tệp mẫu</div>
        </div>
      </div>
      <div v-if="totalValid > 0 || totalInvalid > 0" class="record-of-file flex">
        <div class="record-quantity bgi-valid margin-right-16 text-18 flex">
          Hợp lệ :&nbsp;{{ totalValid }}
        </div>
        <div class="record-quantity invalid bgi-invalid text-18 flex">
          Không hợp lệ :&nbsp;{{ totalInvalid }}
        </div>
      </div>
      <span v-if="totalInvalid > 0" class="text-16"
        >Tải xuống bản ghi bị lỗi
        <span class="link-download" @click="exportInvalidUser"
          >tại đây</span
        ></span
      >
      <div class="popup-button flex">
        <div @click="closeDialog">
          <BaseButton buttonType="white" buttonName="Đóng" />
        </div>
        <div title="Nhập khẩu những bản ghi hợp lệ" @click="importValidUser">
          <BaseButton buttonType="regular" buttonName="Nhập khẩu" />
        </div>
      </div>
    </div>
    <input
      hidden
      type="file"
      ref="fileImport"
      id="fileImport"
      @change="changeFile"
    />
  </div>
</template>

<script>
import axios from "axios";
import BaseApi from '../../../constants/BaseApi'
export default {
  props: ["isHideDialogImport"],
  data() {
    return {
      inputFile: null,
      totalValid: null,
      totalInvalid: null,
      validUsers: [],
      invalidUsers: [],
    };
  },
  methods: {
    /**
     * Đóng dialog nhập khẩu
     * Author: VQBao - 20/9/2022
     */
    closeDialog() {
      this.$emit("hideDialogImport", true);
      this.$refs.fileImport.value = null;
      this.totalValid = null;
      this.totalInvalid = null;
      this.validUsers = [];
      this.invalidUsers = [];
    },

    /**
     * Khi click vào button nhập khẩu, tạo sự kiện click chọn file của input
     */
    importFileOnClick() {
      this.$refs.fileImport.click();
    },

    /**
     * Khi chọn xong file thì thực hiện gọi api lấy dữ liệu số bản ghi hợp lệ, bản ghi lỗi, danh sách bản ghi hợp lệ, danh sách bản ghi lỗi
     * Author: VQBao - 20/9/2022
     */
    async changeFile() {
      var formImport = new FormData();
      this.inputFile = document.getElementById("fileImport").files[0];
      formImport.append("fileImport", this.inputFile);
      if (this.inputFile != null) {
        try {
          await axios.post("https://localhost:44382/api/v1/Users/GetImportQuantity", formImport,{
                headers: {
                  "Content-Type": "multipart/form-data",
                },
              }
            )
            .then((response) => {
              this.totalValid = response.data.TotalValid;
              this.totalInvalid = response.data.TotalInvalid;
              this.validUsers = response.data.ValidUsers;
              this.invalidUsers = response.data.InvalidUsers;
            });
        } catch (error) {
          console.log(error);
        }
      }
    },

    /**
     * Khi click vào tải file mẫu, gọi api tải về file excel nhập liệu mẫu
     * Author: VQBao - 20/9/2022
     */
    exportSampleExcel() {
      document.getElementById("loader").style.display = "block";
      try {
        axios({
          url: "https://localhost:44382/api/v1/Users/ExportSampleExcel",
          method: "GET",
          responseType: "blob",
        }).then((response) => {
          document.getElementById("loader").style.display = "none";
          var fileURL = window.URL.createObjectURL(new Blob([response.data]));
          var fileLink = document.createElement("a");

          fileLink.href = fileURL;
          fileLink.setAttribute("download", "Danh_sach_mau.xlsx");
          document.body.appendChild(fileLink);
          fileLink.click();
        });
      } catch (error) {
        document.getElementById("loader").style.display = "none";
        console.log(error);
      }
    },

    /**
     * Khi click chọn nhập khẩu, gọi api nhập khẩu những bản ghi hợp lệ
     * Author: VQBao - 20/9/2022
     */
    importValidUser() {
      document.getElementById("loader").style.display = "block";
      if (this.validUsers.length > 0) {
        try {
          axios.post(
            "https://localhost:44382/api/v1/Users/Import", this.validUsers,{
              headers: {
                "Content-Type": "application/json",
              },
            }
          )
          .then(() => {
            document.getElementById("loader").style.display = "none";
            this.validUsers = [];
            this.$emit("hideToast", "Nhập khẩu người dùng thành công");
            this.$emit("loadData")
          });
        } catch (error) {
          document.getElementById("loader").style.display = "none";
          console.log(error);
        }
      }
    },

    /**
     * Xuất khẩu ra file excel những bản ghi bị lỗi
     * Author: VQBao - 20/9/2022
     */
    exportInvalidUser() {
      if (this.invalidUsers.length > 0) {
        try {
          this.export(BaseApi.UserApi+"/ExportInvalidUser",this.invalidUsers, "Danh_sach_nguoi_dung_loi.xlsx");
          this.invalidUsers = [];
        } catch (error) {
          console.log(error);
        }
      }
    },
    /**
     * Hàm xuất khẩu chung
     * Author: VQBao - 20/9/2022
     */
    export(urlApi ,users, fileName){
      try {
          axios({
            url: urlApi,
            method: "POST",
            data: users,
            responseType: "blob",
          }).then((response) => {
            var fileURL = window.URL.createObjectURL(new Blob([response.data]));
            var fileLink = document.createElement("a");

            fileLink.href = fileURL;
            fileLink.setAttribute("download", fileName);
            document.body.appendChild(fileLink);
            fileLink.click();
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
.dilog-import {
  position: fixed;
  align-items: center;
  justify-content: center;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.24);
  z-index: 99;
  box-sizing: border-box;
}
.popup-title {
  font-size: 24px;
  margin-bottom: 16px;
}
.dialog-import-content {
  width: 560px;
  background-color: #ffffff;
  position: relative;
  display: flex;
  flex-direction: column;
  padding: 24px 24px;
  box-sizing: border-box;
}
.block-item {
  width: 100%;
  height: 60px;
  box-sizing: border-box;
  background-color: aquamarine;
  font-size: 20px;
  font-weight: 700;
  justify-content: center;
  border: 1px solid;
  border-radius: 4px;
}
.text-18 {
  font-size: 18px;
  /* font-weight: 600; */
}
.example-file {
  border-color: #e0e5e6;
  background-color: var(--primary);
  color: #fff !important;
  cursor: pointer;
  background-size: cover;
}
.upload-file {
  background-color: var(--primary);
  color: #fff;
  margin: 16px 0;
  cursor: pointer;
}
.record-quantity {
  width: 248px;
  height: 60px;
  background-color: var(--primary);
  box-sizing: border-box;
  padding-left: 16px;
  margin-bottom: 8px;
  color: #fff;
  border-radius: 4px;
}
.invalid {
  background-color: rgb(245, 155, 107);
}
.margin-right-16 {
  margin-right: 16px;
}
.text-16 {
  font-size: 16px;
}
.link-download {
  font-weight: 600;
  cursor: pointer;
}

.popup-button {
  justify-content: flex-end;
  margin-top: 24px;
}
.select-file {
  width: 100%;
  height: 200px;
  background-color: #fff;
  border: 1px #ccc;
  border-style: dashed !important;
  box-sizing: border-box;
}
.file-icon {
  width: 50px;
  height: 50px;
  cursor: pointer;
}
.file-title {
  margin-top: 4px;
  color: #757575;
  font-size: 13px;
  cursor: pointer;
}
.file-block {
  width: 100%;
  flex-direction: column;
  justify-content: center;
}
.file-sample {
  margin-top: 8px;
  width: 120px;
  height: 32px;
  justify-content: center;
  border: 1px solid var(--button-primary-normal-border-color);
  border-radius: 4px;
  background-color: var(--button-primary-normal-bg-color);
  box-sizing: border-box;
  color: var(--text-white-primary-color);
  cursor: pointer;
}
.file-sample:hover {
  background-color: var(--button-primary-hover-bg-color);
  border-color: var(--button-primary-hover-border-color);
  color: var(--text-white-secondary-color);
}
span, div {
  font-family: Roboto,Helvetica,Arial,sans-serif !important;
}
.record-of-file {
  margin-top: 8px;
}
</style>