import fetch from '@/utils/fetch'

export function login(method, data) {
  return fetch({
    url: 'Admin',
    method: method,
    params: data
  })
}

export function getInfo(method) {
  return fetch({
    url: 'Admin',
    method: method
  })
}

export function logout() {
  return fetch({
    url: '/user/logout',
    method: 'post'
  })
}
export function despoitVoice(method, data) {
  return fetch({
    url: 'DespoitVoice',
    method: method,
    params: data
  })
}

