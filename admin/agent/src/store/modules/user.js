import { login, logout, getInfo,agentQuickLogon,getDomainLogo } from '@/api/login'
import { getToken, setToken, removeToken } from '@/utils/auth'

const user = {
  state: {
    token: getToken(),
    name: '',
    code:'',
    commission:'',
    avatar: '',
    roles: [],
    userid:0,
    company_logo: '',
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_CODE: (state, code) => {
      state.code = code
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    },
    SET_USERID: (state, userid) => {
      state.userid = userid
    },
    SET_COMMISSION: (state, commission) => {
      state.commission = commission
    },
    SET_COMPANY_LOGO: (state, company_logo) => {
      state.company_logo = company_logo
    }
  },

  actions: {
    // 登录
    Login({ commit }, userInfo) {
      const username = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        login("get",{"code":username, "password":userInfo.password}).then(res => {
          setToken(res.token)
          commit('SET_TOKEN', res.token)
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 一键登录
    AgentQuickLogon({ commit }, userInfo) {
      return new Promise((resolve, reject) => {
        agentQuickLogon("get",userInfo).then(res => {
          setToken(res.token)
          commit('SET_TOKEN', res.token)
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    GetDomainLogo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getDomainLogo("get", { domain: document.domain, type: 1 }).then(res => {
          if (res.data != null) {
            commit('SET_COMPANY_LOGO', res.data.company_logo)
          }
          resolve(res)
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 获取用户信息
    GetInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getInfo("get").then(res => {
          res.data.rolecode="admin";
          commit('SET_ROLES', [res.data.rolecode])
          commit('SET_NAME', res.data.contacts)
          commit('SET_USERID', res.data.id)
          commit('SET_CODE', res.data.name)
          commit('SET_COMMISSION', res.data.commission)
          console.log(res)
          resolve(res)
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        commit('SET_TOKEN', '')
        commit('SET_ROLES', [])
        removeToken()
        resolve()
      })
    },

    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        removeToken()
        resolve()
      })
    }
  }
}

export default user
