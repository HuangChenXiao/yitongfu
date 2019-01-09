import fetch from '@/utils/fetch'

export function getList(method, data) {
  return fetch({
    url: 'Agent',
    method: method,
    params: data
  })
}
export function presentrecord(method, data) {
  return fetch({
    url: 'PresentRecord',
    method: method,
    params: data
  })
}

export function OrderExport(method, data) {
  return fetch({
    url: 'Export/OrderExport.ashx',
    method: method,
    params: data
  })
}
export function auditWithdrawals(method, data) {
  return fetch({
    url: 'AuditWithdrawals',
    method: method,
    params: data
  })
}

export function OrderSumList(method, data) {
  return fetch({
    url: 'OrderSumList',
    method: method,
    params: data
  })
}

export function BusinessOrderaMount(method, data) {
  return fetch({
    url: 'BusinessOrderaMount',
    method: method,
    params: data
  })
}

export function PayAccountOrderAmount(method, data) {
  return fetch({
    url: 'PayAccountOrderAmount',
    method: method,
    params: data
  })
}
export function OrderUpdateByAdmin(method, data) {
  return fetch({
      url: 'OrderUpdateByAdmin',
      method: method,
      params: data
  })
}
