import fetch from '@/utils/fetch'

export function bymerchant(method, data) {
  return fetch({
    url: 'ByMerchant',
    method: method,
    params: data
  })
}