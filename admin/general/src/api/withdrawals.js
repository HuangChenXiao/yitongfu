import fetch from '@/utils/fetch'

export function withdrawals(method, data) {
  return fetch({
    url: 'Withdrawals',
    method: method,
    params: data
  })
}
export function editwithdrawals(method, data) {
  return fetch({
    url: 'Withdrawals',
    method: method,
    data
  })
}