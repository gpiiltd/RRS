import { KeyValuePair } from '../keyValuePair';
import { StateKeyValuePair } from './statekeyvaluepair';

export interface CountryKeyValuePair extends KeyValuePair {
    states: StateKeyValuePair;
  }