import { createStore } from 'vuex'
import user from './modules/user.js'
import userGroup from './modules/userGroup.js'

const store = createStore({
  modules: {
    user,
    userGroup,
  },
})

export default store