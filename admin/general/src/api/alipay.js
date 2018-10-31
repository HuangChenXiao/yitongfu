import fetch from '@/utils/fetch'

export function PayPassList(method, data) {
  return fetch({
    url: 'PayPassList',
    method: method,
    params: data
  })
}