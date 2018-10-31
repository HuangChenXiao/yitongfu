import fetch from '@/utils/fetch'

export function getList(method, data) {
  return fetch({
    url: 'ByMerchant',
    method: method,
    params: data
  })
}