export class DateModel {
  constructor(year: number, month: number, day: number) {
    this.year = year;
    this.month = month;
    this.day = day;
  }

  public year: number;

  public month: number;

  public day: number;

  public static isEqual(a: DateModel, b: DateModel): boolean {
    return this.compare(a, b) === 0;
  }

  public static compare(a: DateModel, b: DateModel): number {
    if (a.year < b.year) return -1;
    else if (a.year > b.year) return 1;
    if (a.month < b.month) return -1;
    else if (a.month > b.month) return 1;
    if (a.day < b.day) return -1;
    else if (a.day > b.day) return 1;
    return 0;
  }

  public clone(): DateModel {
    return new DateModel(this.year, this.month, this.day);
  }
}
