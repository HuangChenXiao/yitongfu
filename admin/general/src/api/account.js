import fetch from '@/utils/fetch'

export function BusinessCheckBalance(method, data) {
  return fetch({
    url: 'BusinessCheckBalance',
    method: method,
    params: data
  })
}