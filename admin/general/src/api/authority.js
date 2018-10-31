import fetch from '@/utils/fetch'

export function admin(method, data) {
  return fetch({
    url: 'Admin',
    method: method,
    params: data
  })
}
export function editadmin(method, data) {
  return fetch({
    url: 'Admin',
    method: method,
    data
  })
}

export function role(method, data) {
  return fetch({
    url: 'Role',
    method: method,
    params: data
  })
}
export function editrole(method, data) {
  return fetch({
    url: 'Role',
    method: method,
    data
  })
}

export function router(method, data) {
  return fetch({
    url: 'Router',
    method: method,
    params: data
  })
}
export function editrouter(method, data) {
  return fetch({
    url: 'Router',
    method: method,
    data
  })
}

export function routers(method, data) {
  return fetch({
    url: 'Routers',
    method: method,
    params: data
  })
}
export function editrouters(method, data) {
  return fetch({
    url: 'Routers',
    method: method,
    data
  })
}