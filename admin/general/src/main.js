import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-default/index.css'
import App from './App'
import router from './router'
import store from './store'
import '@/icons' // icon
import '@/permission' // 权限
import cfg from '@/config/index.js'
import '@/assets/css/element-theme.css'
import '@/assets/css/style.css'
Vue.prototype.cfg = cfg
import NiuLoading from "@/components/loading"
Vue.component('niu-loading', NiuLoading)

Vue.prototype.excel_api=process.env.EXCEL_API
Vue.prototype.baseurl = process.env.BASE_API;
Vue.prototype.basepagesize = 10

Vue.use(ElementUI)

Vue.config.productionTip = false


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