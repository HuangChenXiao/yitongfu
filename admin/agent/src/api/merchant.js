import fetch from '@/utils/fetch'

export function merchant(method, data) {
  return fetch({
    url: 'FaAgent',
    method: method,
    params: data
  })
}
export function editmerchant(method, data) {
  return fetch({
    url: 'FaAgent',
    method: method,
    data
  })
}

export function businessbasic(method, data) {
  return fetch({
    url: 'Businessbasic',
    method: method,
    params: data
  })
}
export function editbusinessbasic(method, data) {
  return fetch({
    url: 'Businessbasic',
    method: method,
    data
  })
}

export function AgentMerchant(method, data) {
  return fetch({
    url: 'AgentMerchant',
    method: method,
    params: data
  })
}
export function AgentBusinessBasinc(method, data) {
  return fetch({
    url: 'AgentBusinessBasinc',
    method: method,
    params: data
  })
}

export function merchantbankcard(method, data) {
  return fetch({
    url: 'FaMerchantBankCard',
    method: method,
    params: data
  })
}
export function editmerchantbankcard(method, data) {
  return fetch({
    url: 'FaMerchantBankCard',
    method: method,
    data
  })
}
export function FaBusinessBasic(method, data) {
  return fetch({
    url: 'FaBusinessBasic',
    method: method,
    params: data
  })
}

export function openpayment(method, data) {
  return fetch({
    url: 'FaOpenPayment',
    method: method,
    params: data
  })
}
export function editopenpayment(method, data) {
  return fetch({
    url: 'FaOpenPayment',
    method: method,
    data
  })
}

export function BusinessPass(method, data) {
  return fetch({
      url: 'BusinessPass',
      method: method,
      params: data
  })
}
export function BusBalance(method, data) {
  return fetch({
      url: 'BusBalance',
      method: method,
      params: data
  })
}
export function UpdatePassword(method, data) {
  return fetch({
    url: 'UpdatePassword',
    method: method,
    params: data
  })
}

export function UnionBalance(method, data) {
  return fetch({
      url: 'UnionBalance',
      method: method,
      params: data
  })
}
export function PayPassList(method, data) {
  return fetch({
    url: 'PayPassList',
    method: method,
    params: data
  })
}
export function MeMember(method, data) {
  return fetch({
      url: 'MeMember',
      method: method,
      params: data
  })
}
export function EditMeMember(method, data) {
  return fetch({
      url: 'MeMember',
      method: method,
      data
  })
}