import fetch from '@/utils/fetch'

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