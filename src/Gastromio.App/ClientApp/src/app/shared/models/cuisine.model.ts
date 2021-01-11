export class CuisineModel {

  constructor(init?: Partial<CuisineModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public id: string;

  public name: string;

  public clone(): CuisineModel {
    return new CuisineModel({
      id: this.id,
      name: this.name
    });
  }

}
