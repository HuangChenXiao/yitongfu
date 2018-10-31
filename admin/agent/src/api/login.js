import fetch from '@/utils/fetch'

export function login(method, data) {
  return fetch({
    url: 'FaAgent',
    method: method,
    params: data
  })
}

export function getInfo(method) {
  return fetch({
    url: 'FaAgent',
    method: method
  })
}

export function agentQuickLogon(method, data) {
  return fetch({
    url: 'AgentQuickLogon',
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
export function despoitVoice(method, data) {
  return fetch({
    url: 'DespoitVoice',
    method: method,
    params: data
  })
}

