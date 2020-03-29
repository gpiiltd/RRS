import { KeyValuePair } from '../keyValuePair';
import { AreaKeyValuePair } from './areakeyvaluepair';

export interface StateKeyValuePair extends KeyValuePair {
    areas: AreaKeyValuePair;
  }