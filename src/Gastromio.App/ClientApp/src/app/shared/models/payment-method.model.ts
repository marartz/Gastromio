export class PaymentMethodModel {

  constructor(init?: Partial<PaymentMethodModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public id: string;

  public name: string;

  public description: string;

  public imageName: string;

  public clone(): PaymentMethodModel {
    return new PaymentMethodModel({
      id: this.id,
      name: this.name,
      description: this.description,
      imageName: this.imageName
    });
  }

}
