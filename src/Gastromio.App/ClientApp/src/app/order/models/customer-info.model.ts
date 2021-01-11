export class CustomerInfoModel {

  constructor(init?: Partial<CustomerInfoModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public givenName: string;

  public lastName: string;

  public street: string;

  public addAddressInfo: string;

  public zipCode: string;

  public city: string;

  public phone: string;

  public email: string;

  public clone(): CustomerInfoModel {
    return new CustomerInfoModel({
      givenName: this.givenName,
      lastName: this.lastName,
      street: this.street,
      addAddressInfo: this.addAddressInfo,
      zipCode: this.zipCode,
      city: this.city,
      phone: this.phone,
      email: this.email
    });
  }

}
