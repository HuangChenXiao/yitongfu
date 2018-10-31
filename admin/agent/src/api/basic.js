import fetch from '@/utils/fetch'

export function merchantcity(method, data) {
  return fetch({
    url: 'MerchantCity',
    method: method,
    params: data
  })
}
export function businesscategory(method, data) {
  return fetch({
    url: 'BusinessCategory',
    method: method,
    params: data
  })
}

export function bankcode(method, data) {
  return fetch({
    url: 'BankCode',
    method: method,
    params: data
  })
}
export function businesscoding(method, data) {
  return fetch({
    url: 'BusinessCoding',
    method: method,
    params: data
  })
}