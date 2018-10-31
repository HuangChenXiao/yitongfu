import Vue from 'vue'
import Router from 'vue-router'
const _import = require('./_import_' + process.env.NODE_ENV)
// in development env not use Lazy Loading,because Lazy Loading too many pages will cause webpack hot update too slow.so only in production use Lazy Loading

/* layout */
import Layout from '../views/layout/Layout'

Vue.use(Router)


/**
* icon : the icon show in the sidebar
* hidden : if `hidden:true` will not show in the sidebar
* redirect : if `redirect:noredirect` will not redirct in the levelbar
* noDropdown : if `noDropdown:true` will not has submenu in the sidebar
* meta : `{ role: ['admin'] }`  will control the page role
**/
// export const constantRouterMap = [
//   { path: '/login', component: _import('login/index'), hidden: true },
//   { path: '/404', component: _import('404'), hidden: true },
//   {
//     path: '/',
//     component: Layout,
//     redirect: '/dashboard',
//     name: 'Dashboard',
//     hidden: true,
//     children: [{ path: 'dashboard', component: _import('dashboard/index') }]
//   },

//   {
//     path: '/example',
//     component: Layout,
//     redirect: 'noredirect',
//     name: 'Example',
//     icon: 'zujian',
//     children: [
//       { path: 'index', name: 'Form', icon: 'zonghe', component: _import('page/form') }
//     ]
//   },

//   {
//     path: '/table',
//     component: Layout,
//     redirect: '/table/index',
//     icon: 'tubiao',
//     noDropdown: true,
//     children: [{ path: 'index', name: 'Table', component: _import('table/index'), meta: { role: ['admin'] } }]
//   },

//   { path: '*', redirect: '/404', hidden: true }
// ]
export const constantRouterMap = [
  { name: 'login', path: '/login', component: _import('login/index'), hidden: true },
  // { name: 'help', path: '/help', component: _import('/help/index'), hidden: true },
  { path: '/404', component: _import('404'), hidden: true },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    name: '管理首页',
    hidden: true,
    children: [{ path: 'dashboard', component: _import('dashboard/index') }]
  },
  { name: 'quicklogon', path: '/quicklogon/:username/:password/:sign', component: _import('quicklogon/index'), hidden: true }
]

export default new Router({
  // mode: 'history', //后端支持可开
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap
})

export const asyncRouterMap = [
  {
    path: '/merchant',
    component: Layout,
    redirect: 'noredirect',
    icon: 'tubiao',
    noDropdown: false,
    name: "商户管理",
    meta: { role: ['admin'] },
    children: [
      // { path: 'index', name: '大商户', component: _import('merchant/index'), meta: { role: ['admin'] } },
      { path: 'business_basic', name: '商户列表', component: _import('merchant/business_basic'), meta: { role: ['admin'] } },
      { path: 'merchant_bank_card', name: '银行卡录入', component: _import('merchant/merchant_bank_card'), meta: { role: ['admin'] } },
      { path: 'business_balance', name: '商户余额', component: _import('merchant/business_balance'), meta: { role: ['admin'] } },
      // { path: 'open_payment', name: '费率修改', component: _import('merchant/open_payment'), meta: { role: ['admin'] } },
    ]
  },
  {
    path: '/order',
    component: Layout,
    redirect: 'noredirect',
    icon: 'tubiao',
    noDropdown: true,
    name: "订单管理",
    children: [
      { path: 'index', name: '订单列表', component: _import('order/index') },
    ]
  },
  {
    path: '/withdrawals',
    component: Layout,
    redirect: 'noredirect',
    icon: 'tubiao',
    noDropdown: false,
    name: "提现管理",
    children: [
      { path: 'index', name: '佣金提现', component: _import('withdrawals/index') },
      { path: 'withdrawals', name: '商户提现记录', component: _import('order/withdrawals') },
    ]
  },

  { path: '*', redirect: '/404', hidden: true }
]
