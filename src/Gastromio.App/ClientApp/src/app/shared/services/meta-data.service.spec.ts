import { TestBed } from '@angular/core/testing';

import { MetaDataService } from './meta-data.service';

describe('MetaDataService', () => {
  let service: MetaDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MetaDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
