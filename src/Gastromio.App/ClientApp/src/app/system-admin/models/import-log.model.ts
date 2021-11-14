import { ImportLogLineModel } from './import-log-line.model';

export class ImportLogModel {
  constructor(init?: Partial<ImportLogModel>) {
    if (init) {
      Object.assign(this, init);
    }
    this.lines = this.lines?.map((line) => new ImportLogLineModel(line));
  }

  public lines: Array<ImportLogLineModel>;

  public clone(): ImportLogModel {
    return new ImportLogModel({
      lines: this.lines?.map((line) => line?.clone()),
    });
  }
}
