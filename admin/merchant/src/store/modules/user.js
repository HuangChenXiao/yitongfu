import { login, logout, getInfo, merchantQuickLogon, getDomainLogo } from '@/api/login'
import { getToken, setToken, removeToken } from '@/utils/auth'

const user = {
  state: {
    token: getToken(),
    user_name: '',
    user_id: 0,
    appid: "",
    appsecret: "",
    avatar: '',
    roles: [],
    company_logo: '',
    alipaybalance: 0,
    balance: 0,
    wechatbalance: 0,
    businesspasstype:''
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_USER_NAME: (state, user_name) => {
      state.user_name = user_name
    },
    SET_USER_CODE: (state, user_code) => {
      state.user_code = user_code
    },
    SET_USER_ID: (state, user_id) => {
      state.user_id = user_id
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    },
    SET_APPID: (state, appid) => {
      state.appid = appid
    },
    SET_APPSECRET: (state, appsecret) => {
      state.appsecret = appsecret
    },
    SET_COMPANY_LOGO: (state, company_logo) => {
      state.company_logo = company_logo
    },
    ALIPAYBALANCE: (state, alipaybalance) => {
      state.alipaybalance = alipaybalance
    },
    BALANCE: (state, balance) => {
      state.balance = balance
    },
    WECHATBALANCE: (state, wechatbalance) => {
      state.wechatbalance = wechatbalance
    }, 
    BUSINESSPASSTYPE: (state, businesspasstype) => {
      state.businesspasstype = businesspasstype
    }
  },

  actions: {
    // 登录
    Login({ commit }, userInfo) {
      const username = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        login("get", { "code": username, "password": userInfo.password }).then(res => {
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
        getDomainLogo("get", { domain: document.domain, type: 2 }).then(res => {
          if (res.data != null) {
            commit('SET_COMPANY_LOGO', res.data.company_logo)
          }
          resolve(res)
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 一键登录
    MerchantQuickLogon({ commit }, userInfo) {
      return new Promise((resolve, reject) => {
        debugger
        merchantQuickLogon("get", userInfo).then(res => {
          setToken(res.token)
          commit('SET_TOKEN', res.token)
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 获取用户信息
    GetInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getInfo("get").then(res => {
          res.data.rolecode = "admin";
          commit('SET_ROLES', [res.data.rolecode])
          commit('SET_USER_CODE', res.data.merchantName)
          commit('SET_USER_NAME', res.data.shortName)
          commit('SET_USER_ID', res.data.id)
          commit('SET_APPID', res.data.appId)
          commit('SET_APPSECRET', res.data.appSecret)
          commit('ALIPAYBALANCE', res.data.alipaybalance)
          commit('BALANCE', res.data.balance)
          commit('WECHATBALANCE', res.data.wechatbalance)
          commit('BUSINESSPASSTYPE', res.data.businesspasstype)
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
