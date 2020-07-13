using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCatsDomain.Entities
{
    public class Breeds
    {
        /// <summary>
        /// Construtor sem parâmetros: Necessário para o framework ORM.
        /// Ele é protegido, para que não se permita instanciar o objeto as propriedades obrigatórias
        /// </summary>
        protected Breeds()
        {
        }

        /// <summary>
        /// Construtor com as propriedades obrigatórias para o objeto existir
        /// </summary>
        /// <param name="breedsId"></param>
        /// <param name="name"></param>
        public Breeds(string breedsId, string name)
        {
            BreedsId = breedsId;
            Name = name;
        }

        public string BreedsId { get; private set; }
        public string Name { get; private set; }
        public string Origin { get; private set; }
        public string Temperament { get; private set; }
        public string Description { get; private set; }

        public ICollection<ImageUrl> Images { get; set; } = new List<ImageUrl>();

        // Métodos para atribuir informação a propriedade, validando a informação.

        public void SetBreedsId(string id)
        {
            if (IdIsValid(id))
                BreedsId = id;
        }

        public void SetName(string name)
        {
            if (NameIsValid(name))
                Name = name;
        }

        public void SetOrigin(string origin)
        {
            if (OriginIsValid(origin))
                Origin = origin;
        }

        public void SetTemperament(string temperament)
        {
            if (TemperamentIsValid(temperament))
                Temperament = temperament;
        }

        public void SetDescription(string description)
        {
            if (DescriptionIsValid(description))
                Description = description;
        }

        /// <summary>
        /// Valida se o objeto está com as informações necessárias para ser persistido na base de dados
        /// </summary>
        /// <returns></returns>
        public bool IsValid() =>
            IdIsValid(BreedsId) &&
            NameIsValid(Name) &&
            OriginIsValid(Origin) &&
            TemperamentIsValid(Temperament) &&
            DescriptionIsValid(Description);

        // Métodos privados para consistir informações obrigatórias do objeto

        bool IdIsValid(string breedsId) => (!string.IsNullOrEmpty(breedsId) && breedsId.Length <= 80);
        bool NameIsValid(string name) => (!string.IsNullOrEmpty(name) && name.Length <= 255);
        bool OriginIsValid(string origin) => string.IsNullOrEmpty(origin) ? true : origin.Length <= 255;
        bool TemperamentIsValid(string temperament) => string.IsNullOrEmpty(temperament) ? true : temperament.Length <= 255;
        bool DescriptionIsValid(string description) => string.IsNullOrEmpty(description) ? true : description.Length <= 1024;
    }
}
