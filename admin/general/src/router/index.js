import Vue from 'vue'
import Router from 'vue-router'
const _import = require('./_import_' + process.env.NODE_ENV)
// in development env not use Lazy Loading,because Lazy Loading too many pages will cause webpack hot update too slow.so only in production use Lazy Loading

/* layout */
import Layout from '../views/layout/Layout'

Vue.use(Router)


import { router } from "@/api/authority.js";

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
]

export default new Router({
  // mode: 'history', //后端支持可开
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap
})

// export const asyncRouterMap = [
//   {
//     path: '/authority',
//     component: Layout,
//     redirect: 'noredirect',
//     icon: 'tubiao',
//     noDropdown: false,
//     name: "用户管理",
//     meta: { role: ['admin'] },
//     children: [
//       { path: 'admin', name: '用户列表', component: _import('authority/admin'), meta: { role: ['admin'] } },
//       { path: 'role', name: '角色列表', component: _import('authority/role'), meta: { role: ['admin'] } },
//       { path: 'router', name: '主路由', component: _import('authority/router'), meta: { role: ['admin'] } },
//       { path: 'routers', name: '子路由', component: _import('authority/routers'), meta: { role: ['admin'] } },
//     ]
//   },

//   { path: '*', redirect: '/404', hidden: true }
// ]

//获取后台动态路由
var runAsyncRouterMap = function () {
  var RouterMap = new Promise(function (resolve, reject) {
    router('get').then(res => {
          var rdata = res.data;
          if (rdata.length > 0) {
              for (var i = 0; i < rdata.length; i++) {
                  rdata[i].component = Layout;
                  if (rdata[i].meta) {
                      rdata[i].meta = { role: rdata[i].meta.split(',') };
                  }
                  if (rdata[i].children) {
                      for (var j = 0; j < rdata[i].children.length; j++) {
                          rdata[i].children[j].component = _import(rdata[i].children[j].component);
                          if (rdata[i].children[j].meta) {
                              rdata[i].children[j].meta = { role: rdata[i].children[j].meta.split(',') };
                          }
                      }
                  }
              }
          }
          rdata.push({ path: '*', redirect: '/404', hidden: true })
          resolve(rdata)
      });
  });
  return RouterMap;
}

export {
  runAsyncRouterMap
}