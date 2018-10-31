import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-default/index.css'
import App from './App'
import router from './router'
import store from './store'
import '@/icons' // icon
import '@/permission' // 权限
import md5 from 'js-md5';
import '@/assets/css/element-theme.css'
import cfg from '@/config/index.js'
Vue.prototype.cfg = cfg

Vue.prototype.baseurl=process.env.BASE_API;
Vue.prototype.excel_api=process.env.EXCEL_API

Vue.use(ElementUI)

Vue.config.productionTip = false

router.beforeEach((to, from, next) => {
  store.dispatch("GetDomainLogo").then(res=>{
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
