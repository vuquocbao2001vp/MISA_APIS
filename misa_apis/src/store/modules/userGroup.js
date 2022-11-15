import BaseApi from '../../constants/BaseApi.js'
import axios from 'axios';
const state = {
  userGroups: [],
  userGroupPaging: {
    totalRecords: null,
    totalPages: null,
    pageSize: null,
    pageNumber: null,
    recordStart: null,
    recordEnd: null,
  },
  groupDataSelected: {
    groupSelected: {},
    userOfGroupSelected: [],
    memberPaging: {
      totalRecords: null,
      totalPages: null,
      pageSize: null,
      pageNumber: null,
      recordStart: null,
      recordEnd: null,
    }
  },
};

const getters = {
  userGroups(state) {
    return state.userGroups;
  },
  userGroupPaging(state) {
    return state.userGroupPaging;
  },
  groupSelected(state) {
    return state.groupDataSelected.groupSelected;
  },
  userOfGroupSelected(state) {
    return state.groupDataSelected.userOfGroupSelected;
  },
  memberPaging(state){
    return state.groupDataSelected.memberPaging;
  }
};
const mutations = {
  /**
   * Gán dữ liệu nhóm người dùng lấy về từ api cho state
   * @param {*} state
   * @param {*} response: dữ liệu lấy từ api
   * Author: VQBao - 8/9/2022
   */
  getUserGroupPaging(state, response) {
    state.userGroups = response.Data;
    state.userGroupPaging.totalRecords = response.TotalRecords;
    state.userGroupPaging.totalPages = response.TotalPages;
    state.userGroupPaging.pageSize = response.PageSize;
    state.userGroupPaging.pageNumber = response.PageNumber;
    state.userGroupPaging.recordStart = response.RecordStart;
    state.userGroupPaging.recordEnd = response.RecordEnd;
  },
  /**
   * Lấy nhóm người dùng theo id
   * Author: VQBao - 8/9/2022
   */
  getGroupById(state, response) {
    state.groupDataSelected.groupSelected = response;
  },
  /**
   * Phân trang người dùng thuộc nhóm người dùng khi tìm kiếm
   * @param {*} state 
   * @param {*} response 
   * Author: VQBao - 8/9/2022
   */
  getUserOfGroupPaging(state, response) {
    state.groupDataSelected.userOfGroupSelected = response.Data;
    state.groupDataSelected.memberPaging.totalRecords = response.TotalRecords;
    state.groupDataSelected.memberPaging.totalPages = response.TotalPages;
    state.groupDataSelected.memberPaging.recordStart = response.RecordStart;
    state.groupDataSelected.memberPaging.recordEnd = response.RecordEnd;
    state.groupDataSelected.memberPaging.pageSize = response.PageSize;
    state.groupDataSelected.memberPaging.pageNumber = response.PageNumber;
  },
  getAllGroup(state, response){
    state.userGroups = response;
  }
};

const actions = {
  /**
   * Lấy dữ liệu phân trang nhóm người dùng từ api
   * @param {*} param0
   * @param {*} param1
   * VQBao - 8/9/2022
   */
  async getUserGroupData({ commit }, { pageSize, pageNumber, searchKey }) {
    document.getElementById("loader").style.display = "block";
    try {
      await axios
        .get(BaseApi.UserGroupApi + `/Filter`, {
          params: {
            pageSize: pageSize,
            pageNumber: pageNumber,
            searchKey: searchKey,
          },
        })
        .then((response) => {
          commit("getUserGroupPaging", response.data);
          document.getElementById("loader").style.display = "none";
        });
    } catch (error) {
      document.getElementById("loader").style.display = "none";
      console.log(error);
    }
  },
  /**
   * Lấy dữ liệu nhóm người dùng theo id
   * Author: VQBao - 8/9/2022
   */
  async getGroupDataById({ commit }, groupSelectedId) {
    try {
      await axios
        .get(BaseApi.UserGroupApi + `/` + groupSelectedId)
        .then((response) => {
          commit("getGroupById", response.data);
        });
    } catch (error) {
      console.log(error);
    }
  },
  /**
   * Phân trang tìm kiếm thành viên thuộc nhóm người dùng
   * Author: VQBao - 8/9/2022
   */
  async getUserDataOfGroupPaging({ commit }, {pageSize, pageNumber, userGroupId, searchByText, searchByPosition, MISACode} ) {
    document.getElementById("loader").style.display = "block";
    try {
      await axios
        .get(BaseApi.UserGroupApi + `/MemberOfGroupPaging`, {
          params: {
            pageSize: pageSize,
            pageNumber: pageNumber,
            userGroupId: userGroupId,
            searchByText: searchByText,
            searchByPosition: searchByPosition,
            MISACode: MISACode,
          },
        })
        .then((response) => {
          commit("getUserOfGroupPaging", response.data);
          document.getElementById("loader").style.display = "none";
        });
    } catch (error) {
      document.getElementById("loader").style.display = "none";
      console.log(error);
    }
  },
  /**
   * Lấy tất cả nhóm người dùng từ api
   * Author: VQBao - 12/9/2022
   */
  async getAllGroupData({commit}){
    document.getElementById("loader").style.display = "block";
    try {
      await axios
        .get(BaseApi.UserGroupApi)
        .then((response) => {
          commit("getAllGroup", response.data);
          document.getElementById("loader").style.display = "none";
        });
    } catch (error) {
      document.getElementById("loader").style.display = "none";
      console.log(error);
    }
  },
};

export default {
  state,
  getters,
  mutations,
  actions,
};
