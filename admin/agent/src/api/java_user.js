import fetch from '@/utils/external_fetch'

export function merchBaseDataRegister(method, data) {
  return fetch({
    url: 'MLMerchant/merchBaseDataRegister/merchSet.doAdminJJ',
    method: method,
    params: data
  })
}

export function merchOpenBusiness(method, data) {
  return fetch({
    url: 'MLMerchant/merchOpenBusiness/getOpenBusiness.doAdminJJ',
    method: method,
    params: data
  })
}

export function merchBankCardRegister(method, data) {
  return fetch({
    url: 'MLMerchant/merchBankCardRegister/getBankCard.doAdminJJ',
    method: method,
    params: data
  })
}