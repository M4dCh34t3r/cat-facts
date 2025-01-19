import * as t from 'io-ts';

const dateTimeType = t.union([t.number, t.string]);

type DTOsKeys = keyof typeof dtos;
type ModelsKeys = keyof typeof models;

type Key = DTOsKeys | ModelsKeys;
type Object<T> = {
  [k in Key]: Record<k, T>;
}[Key];

const dtos = {
  serviceExceptionDTO: t.type({
    category: t.string,
    title: t.string,
    text: t.string
  })
};

const models = {
  apiFact: t.type({
    id: t.string,
    text: t.string,
    insertedAt: dateTimeType,
    occurrenceCount: t.number,
    likeCount: t.number,
    dislikeCount: t.number
  }),
  paginated: t.type({
    items: t.UnknownArray,
    pageIndex: t.number,
    pageSize: t.number,
    totalItems: t.number,
    totalPages: t.number
  })
};

const assertMapping: Record<Key, (typeStr: string, obj: any) => any> = {
  // DTOs
  serviceExceptionDTO: (key, obj) =>
    assert(dtos.serviceExceptionDTO, 'ServiceExceptionDTO', obj[key]),
  // Models
  apiFact: (key, obj) => assert(models.apiFact, 'APIFact', obj[key]),
  paginated: (key, obj) => assert(models.paginated, 'Paginated', obj[key])
};

export function assertType<T>(obj: Object<T>): T {
  const typeStr = Object.keys(obj)[0];

  if (import.meta.env.PROD) return obj[typeStr as keyof Object<T>];

  const mapping = assertMapping[typeStr as Key];
  return mapping ? mapping(typeStr, obj) : ignore(typeStr, obj);
}

function assert<T>(typeIO: t.Type<T>, typeStr: string, obj: Object<T>) {
  const isValid = (value: Object<T>) => typeIO.decode(value)._tag === 'Right';

  const generateMismatchMessage = (value: any): string => {
    const fields = Object.keys(value)
      .map((key) => `        ${key}: ${typeof value[key]};`)
      .join('\n');
    return `An object interpreted as\n\n    interface ${typeStr} {\n${fields}\n    }\n\nIsn't of type "${typeStr}"`;
  };

  if (Array.isArray(obj)) {
    if (obj.length === 0 || isValid(obj[0])) return obj as T;
    console.warn(generateMismatchMessage(obj[0]));
  } else if (isValid(obj)) return obj as T;
  else console.warn(generateMismatchMessage(obj));

  return obj as T;
}

function ignore<T>(typeStr: string, obj: Object<T>) {
  console.warn(`The types of "${typeStr}" couldn't be validated`);
  return obj as T;
}
