export class FailureResult {

  constructor(generalErrors: string[], componentErrors: object) {
    this.generalErrors = generalErrors || [];
    this.componentErrors = componentErrors || {};
  }

  public static backendKey = 'backend';

  public statusCode: number;

  public componentErrors: object;

  public generalErrors: string[];

  static createFromString(error: string): FailureResult {
    return new FailureResult([error], {});
  }

  public addComponentErrorsToFormControls(formControls: object, separator: string = ' ') {
    // form control names need to be (case-sensitive!) equal to the property names in the C# models.
    Object.keys(this.componentErrors).forEach((key) => {
      const control = formControls[key];
      const joinedComponentErrors = this.componentErrors[key].join(separator);
      if (control?.errors) {
        control[FailureResult.backendKey] = joinedComponentErrors;
      } else {
        const tmpError = {};
        tmpError[FailureResult.backendKey] = joinedComponentErrors;
        control.setErrors(tmpError);
      }
    });
  }

  // the following functions are just temporary helpers to quickly join messages.
  // instead each message should be rendered in a separate div/span/p.
  // tslint:disable-next-line:max-line-length
  // right now it would work for generalErrors, but not for componentErrors, because generalErrors are encapsuled to a separate component and therefore have a uniform style
  // (using 'white-space: pre-wrap' as workarount to support line-breaks).
  // in order to get it working for componentErrors these also need to be refactored to a separate component.

  public getJoinedGeneralErrors(separator: string = ', '): string {
    return this.generalErrors.join(separator);
  }

  public getJoinedComponentErrors(separator: string = ', '): string {
    const messagesPerComnponent = [];
    Object.keys(this.componentErrors).forEach(key => {
      messagesPerComnponent.push(this.componentErrors[key].join(separator));
    });
    if (messagesPerComnponent.length > 0) {
      return messagesPerComnponent.join(separator);
    }
    return '';
  }

  public getJoinedErrors(collectionsSep: string = ', ', generalSep: string = ', ', componentSep: string = ', '): string {
    return [this.getJoinedGeneralErrors(generalSep), this.getJoinedComponentErrors(componentSep)].filter(Boolean).join(collectionsSep);
  }

}
