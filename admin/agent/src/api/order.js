import fetch from '@/utils/fetch'

export function getList(method, data) {
  return fetch({
    url: 'ByAgent',
    method: method,
    params: data
  })
}

export function AgPresentRecord(method, data) {
  return fetch({
    url: 'AgPresentRecord',
    method: method,
    params: data
  })
}

export function auditWithdrawals(method, data) {
  return fetch({
    url: 'AuditWithdrawals',
    method: method,
    params:data
  })
}
export function BusinessPass(method, data) {
  return fetch({
      url: 'BusinessPass',
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
export function GetNotifyadminInfo(method, data) {
  return fetch({
      url: 'GetNotifyadminInfo',
      method: method,
      params: data
  })
}
