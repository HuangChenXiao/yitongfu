import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-default/index.css'
import App from './App'
import router from './router'
import store from './store'
import '@/icons' // icon
import '@/permission' // 权限
import '@/assets/css/element-theme.css'
import cfg from '@/config/index.js'

Vue.prototype.cfg = cfg;
import VueQriously from 'vue-qriously'
Vue.use(VueQriously)

Vue.use(ElementUI)

Vue.config.productionTip = false

Vue.prototype.node_env = process.env.NODE_ENV;
Vue.prototype.java_baseurl = process.env.JAVA_API;

router.beforeEach((to, from, next) => {
  store.dispatch("GetDomainLogo").then(res => {
    next()
  })
})

//全局过滤器
import * as custom from '@/utils/filter'

Object.keys(custom).forEach(key => {
  Vue.filter(key, custom[key])
})

new Vue({
  el: '#app',
  router,
  store,
  template: '<App/>',
  components: { App }
})
