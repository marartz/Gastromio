export class UserModel {
  constructor(init?: Partial<UserModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public id: string;

  public role: string;

  public email: string;

  public clone(): UserModel {
    return new UserModel({
      id: this.id,
      role: this.role,
      email: this.email,
    });
  }
}
