export class FailureResult {
  constructor(code: string, message: string) {
    this.code = code;
    this.message = message;
  }

  public code: string;

  public message: string;

  static createFromString(error: string): FailureResult {
    return new FailureResult('InternalServerError', error);
  }
}
