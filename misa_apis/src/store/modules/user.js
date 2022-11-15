import axios from 'axios'
import BaseApi from '../../constants/BaseApi.js'

const state = {
  users: [],
  userPaging: {
    totalRecords: null,
    totalPages: null,
    pageSize: null,
    pageNumber: null,
    recordStart: null,
    recordEnd: null,
  },
  userDataSelected: {
    user: {},
    groupOfUser: [],
  },
  jobPosition: [],
  organizationUnit: [],
  userGrid: {
    defaultOption: [],
    userOption: [],
  }
}

const getters = {
    users(state){
      return state.users;
    },
    userPaging(state){
      return state.userPaging;
    },
    userSelected(state){
      return state.userDataSelected.user;
    },
    groupOfUserSelected(state){
      return state.userDataSelected.groupOfUser;
    },
    jobPosition(state){
      return state.jobPosition;
    },
    organizationUnit(state){
      return state.organizationUnit;
    },
    userGrid(state){
      return state.userGrid;
    }
}
const mutations = {
  /**
   * Lấy dữ liệu người dùng phân trang từ api gán cho state
   * @param {*} state 
   * @param {*} response: Dữ liệu trả về từ api
   * Author: VQBao - 8/9/2022
   */
  getUserPaging(state, response) {
    state.users = response.Data;
    state.userPaging.totalRecords = response.TotalRecords;
    state.userPaging.totalPages = response.TotalPages;
    state.userPaging.pageSize = response.PageSize;
    state.userPaging.pageNumber = response.PageNumber;
    state.userPaging.recordStart = response.RecordStart;
    state.userPaging.recordEnd = response.RecordEnd;
  },
  /**
   * Lấy người dùng theo id
   * Author: VQBao - 8/9/2022
   */
  getUserById(state, response){
    state.userDataSelected.user = response.User;
    state.userDataSelected.groupOfUser = response.UserGroup;
  },
  /**
   * Lấy toàn bộ chức vụ của người dùng
   * @param {*} state 
   * @param {*} response 
   * Author: VQBao - 8/9/2022
   */
  getJobPosition(state, response){
    state.jobPosition = response;
  },
  /**
   * Lấy toàn bộ phòng ban của người dùng
   * @param {*} state 
   * @param {*} response 
   * Author: VQBao - 8/9/2022
   */
  getOrganizationUnit(state, response){
    state.organizationUnit = response;
  },
  /**
   * Lấy tùy chỉnh cột của người dùng
   * @param {*} state 
   * @param {*} response 
   * Author: VQBao - 15/9/2022
   */
  getUserGrid(state, response){
    state.userGrid.defaultOption = response[0].OptionDefault;
    state.userGrid.userOption = response[0].UserOption;
  }
}

const actions = {
  /**
   * Lấy dữ liệu người dùng phân trang từ api
   * Author: VQBao - 6/9/2022
   */
  async getUserData({ commit }, { pageSize, pageNumber, searchByText, searchByGroup}) {
    document.getElementById("loader").style.display = "block";
    try {
        await axios
          .get(BaseApi.UserApi + `/Filter`,{
            params: {
                pageSize: pageSize,
                pageNumber: pageNumber,
                searchByText: searchByText,
                searchByGroup: searchByGroup
            }
          }
          )
          .then((response) => {
            commit("getUserPaging", response.data);
            document.getElementById("loader").style.display = "none";
          });
    } catch (error) {
      console.log(error);
    }
  },
  /**
   * Lấy dữ liệu người dùng theo id
   * Author: VQBao - 8/9/2022
   */
  async getUserDataById({commit}, userSelectedId) {
    document.getElementById("loader").style.display = "block";
    try {
      await axios.get(BaseApi.UserApi + `/` + userSelectedId)
        .then((response) => {
          commit("getUserById", response.data);
          document.getElementById("loader").style.display = "none";
        })
    } catch (error) {
      console.log(error);
    }
  },
  /**
   * Lấy chức vụ người dùng từ api
   * Author: VQBao - 8/9/2022
   */
  async getJobPositionData({commit}) {
    try {
      await axios.get(BaseApi.JobPositionApi)
        .then((response) => {
          commit("getJobPosition", response.data)
        })
    } catch (error) {
      console.log(error);
    }
  },
  /**
   * Lấy toàn bộ dữ liệu phòng ban từ api
   * @param {*} param0 
   * Author: VQBao - 8/9/2022
   */
  async getOrganizationUnitData({commit}) {
    try {
      await axios.get(BaseApi.OrganizationUnitApi)
        .then((response) => {
          commit("getOrganizationUnit", response.data)
        })
    } catch (error) {
      console.log(error);
    }
  },
  /**
   * Lấy tùy chỉnh cột của người dùng từ api
   * @param {*} param0 
   * @param {*} userId 
   * Author: VQBao - 8/9/2022
   */
  async getUserGridData({commit}, userId) {
    try {
      await axios.get(BaseApi.UserApi +`/GetUserGrid/`+userId)
        .then((response) => {
          commit("getUserGrid", response.data)
        })
    } catch (error) {
      console.log(error);
    }
  },
}

export default {
    state,
    getters,
    mutations,
    actions
}
