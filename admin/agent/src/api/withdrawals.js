import fetch from '@/utils/fetch'

export function withdrawalsbyagid(method, data) {
  return fetch({
    url: 'ByAgent',
    method: method,
    params: data
  })
}
export function editwithdrawalsbyagid(method, data) {
  return fetch({
    url: 'ByAgent',
    method: method,
    data
  })
}