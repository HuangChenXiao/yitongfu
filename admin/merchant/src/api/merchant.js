import fetch from '@/utils/fetch'

export function famerchant(method, data) {
  return fetch({
    url: 'FaMerchant',
    method: method,
    params: data
  })
}
export function editfamerchant(method, data) {
  return fetch({
    url: 'FaMerchant',
    method: method,
    data
  })
}
export function getmerchantcard(method, data) {
  return fetch({
    url: 'GetMerchantCard',
    method: method,
    params: data
  })
}


export function editmerchantbankcard(method, data) {
  return fetch({
      url: 'MerchantBankCard',
      method: method,
      data
  })
}
export function GetBankCard(method, data) {
  return fetch({
      url: 'GetBankCard',
      method: method,
      params: data
  })
}
export function businessbasic(method, data) {
  return fetch({
      url: 'Businessbasic',
      method: method,
      params: data
  })
}
export function SendAuditWithdrawals(method, data) {
  return fetch({
      url: 'SendAuditWithdrawals',
      method: method,
      params: data
  })
}

export function BusinessPass(method, data) {
  return fetch({
      url: 'BusinessPass',
      method: method,
      params: data
  })
}
export function BusBalance(method, data) {
  return fetch({
      url: 'BusBalance',
      method: method,
      params: data
  })
}
export function GetByBusiness(method, data) {
  return fetch({
      url: 'GetByBusiness',
      method: method,
      params: data
  })
}
export function UpdatePassword(method, data) {
  return fetch({
    url: 'UpdatePassword',
    method: method,
    params: data
  })
}