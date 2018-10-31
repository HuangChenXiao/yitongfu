import fetch from '@/utils/fetch'

export function login(method, data) {
  return fetch({
    url: 'ByMerchant',
    method: method,
    params: data
  })
}

export function getInfo(method) {
  return fetch({
    url: 'ByMerchant',
    method: method
  })
}

export function merchantQuickLogon(method, data) {
  return fetch({
    url: 'BusinessBasicQuickLogon',
    method: method,
    params: data
  })
}

export function getDomainLogo(method, data) {
  return fetch({
    url: 'getDomainLogo',
    method: method,
    params: data
  })
}