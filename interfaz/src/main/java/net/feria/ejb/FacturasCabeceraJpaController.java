/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.ejb;

import java.io.Serializable;
import javax.persistence.Query;
import javax.persistence.EntityNotFoundException;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import net.feria.entity.Cliente;
import net.feria.entity.DetalleFactura;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.NoResultException;
import net.feria.ejb.exceptions.IllegalOrphanException;
import net.feria.ejb.exceptions.NonexistentEntityException;
import net.feria.entity.FacturasCabecera;

/**
 *
 * @author Deth1
 */
public class FacturasCabeceraJpaController implements Serializable {

    public FacturasCabeceraJpaController(EntityManagerFactory emf) {
        this.emf = emf;
    }
    private EntityManagerFactory emf = null;

    public EntityManager getEntityManager() {
        return emf.createEntityManager();
    }

    public FacturasCabecera create(FacturasCabecera facturasCabecera) {
        if (facturasCabecera.getDetalleFacturaCollection() == null) {
            facturasCabecera.setDetalleFacturaCollection(new ArrayList<DetalleFactura>());
        }
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            Cliente codigoCliente = facturasCabecera.getCodigoCliente();
            if (codigoCliente != null) {
                codigoCliente = em.getReference(codigoCliente.getClass(), codigoCliente.getCodigoCliente());
                facturasCabecera.setCodigoCliente(codigoCliente);
            }
            Collection<DetalleFactura> attachedDetalleFacturaCollection = new ArrayList<DetalleFactura>();
            for (DetalleFactura detalleFacturaCollectionDetalleFacturaToAttach : facturasCabecera.getDetalleFacturaCollection()) {
                detalleFacturaCollectionDetalleFacturaToAttach = em.getReference(detalleFacturaCollectionDetalleFacturaToAttach.getClass(), detalleFacturaCollectionDetalleFacturaToAttach.getCodigoDetalleFactura());
                attachedDetalleFacturaCollection.add(detalleFacturaCollectionDetalleFacturaToAttach);
            }
            facturasCabecera.setDetalleFacturaCollection(attachedDetalleFacturaCollection);
            em.persist(facturasCabecera);
            if (codigoCliente != null) {
                codigoCliente.getFacturasCabeceraCollection().add(facturasCabecera);
                codigoCliente = em.merge(codigoCliente);
            }
            for (DetalleFactura detalleFacturaCollectionDetalleFactura : facturasCabecera.getDetalleFacturaCollection()) {
                FacturasCabecera oldCodigoFacturaOfDetalleFacturaCollectionDetalleFactura = detalleFacturaCollectionDetalleFactura.getCodigoFactura();
                detalleFacturaCollectionDetalleFactura.setCodigoFactura(facturasCabecera);
                detalleFacturaCollectionDetalleFactura = em.merge(detalleFacturaCollectionDetalleFactura);
                if (oldCodigoFacturaOfDetalleFacturaCollectionDetalleFactura != null) {
                    oldCodigoFacturaOfDetalleFacturaCollectionDetalleFactura.getDetalleFacturaCollection().remove(detalleFacturaCollectionDetalleFactura);
                    oldCodigoFacturaOfDetalleFacturaCollectionDetalleFactura = em.merge(oldCodigoFacturaOfDetalleFacturaCollectionDetalleFactura);
                }
            }
            em.getTransaction().commit();

            return facturasCabecera;

        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void edit(FacturasCabecera facturasCabecera) throws IllegalOrphanException, NonexistentEntityException, Exception {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            FacturasCabecera persistentFacturasCabecera = em.find(FacturasCabecera.class, facturasCabecera.getCodigoFactura());
            Cliente codigoClienteOld = persistentFacturasCabecera.getCodigoCliente();
            Cliente codigoClienteNew = facturasCabecera.getCodigoCliente();
            Collection<DetalleFactura> detalleFacturaCollectionOld = persistentFacturasCabecera.getDetalleFacturaCollection();
            Collection<DetalleFactura> detalleFacturaCollectionNew = facturasCabecera.getDetalleFacturaCollection();
            List<String> illegalOrphanMessages = null;
            for (DetalleFactura detalleFacturaCollectionOldDetalleFactura : detalleFacturaCollectionOld) {
                if (!detalleFacturaCollectionNew.contains(detalleFacturaCollectionOldDetalleFactura)) {
                    if (illegalOrphanMessages == null) {
                        illegalOrphanMessages = new ArrayList<String>();
                    }
                    illegalOrphanMessages.add("You must retain DetalleFactura " + detalleFacturaCollectionOldDetalleFactura + " since its codigoFactura field is not nullable.");
                }
            }
            if (illegalOrphanMessages != null) {
                throw new IllegalOrphanException(illegalOrphanMessages);
            }
            if (codigoClienteNew != null) {
                codigoClienteNew = em.getReference(codigoClienteNew.getClass(), codigoClienteNew.getCodigoCliente());
                facturasCabecera.setCodigoCliente(codigoClienteNew);
            }
            Collection<DetalleFactura> attachedDetalleFacturaCollectionNew = new ArrayList<DetalleFactura>();
            for (DetalleFactura detalleFacturaCollectionNewDetalleFacturaToAttach : detalleFacturaCollectionNew) {
                detalleFacturaCollectionNewDetalleFacturaToAttach = em.getReference(detalleFacturaCollectionNewDetalleFacturaToAttach.getClass(), detalleFacturaCollectionNewDetalleFacturaToAttach.getCodigoDetalleFactura());
                attachedDetalleFacturaCollectionNew.add(detalleFacturaCollectionNewDetalleFacturaToAttach);
            }
            detalleFacturaCollectionNew = attachedDetalleFacturaCollectionNew;
            facturasCabecera.setDetalleFacturaCollection(detalleFacturaCollectionNew);
            facturasCabecera = em.merge(facturasCabecera);
            if (codigoClienteOld != null && !codigoClienteOld.equals(codigoClienteNew)) {
                codigoClienteOld.getFacturasCabeceraCollection().remove(facturasCabecera);
                codigoClienteOld = em.merge(codigoClienteOld);
            }
            if (codigoClienteNew != null && !codigoClienteNew.equals(codigoClienteOld)) {
                codigoClienteNew.getFacturasCabeceraCollection().add(facturasCabecera);
                codigoClienteNew = em.merge(codigoClienteNew);
            }
            for (DetalleFactura detalleFacturaCollectionNewDetalleFactura : detalleFacturaCollectionNew) {
                if (!detalleFacturaCollectionOld.contains(detalleFacturaCollectionNewDetalleFactura)) {
                    FacturasCabecera oldCodigoFacturaOfDetalleFacturaCollectionNewDetalleFactura = detalleFacturaCollectionNewDetalleFactura.getCodigoFactura();
                    detalleFacturaCollectionNewDetalleFactura.setCodigoFactura(facturasCabecera);
                    detalleFacturaCollectionNewDetalleFactura = em.merge(detalleFacturaCollectionNewDetalleFactura);
                    if (oldCodigoFacturaOfDetalleFacturaCollectionNewDetalleFactura != null && !oldCodigoFacturaOfDetalleFacturaCollectionNewDetalleFactura.equals(facturasCabecera)) {
                        oldCodigoFacturaOfDetalleFacturaCollectionNewDetalleFactura.getDetalleFacturaCollection().remove(detalleFacturaCollectionNewDetalleFactura);
                        oldCodigoFacturaOfDetalleFacturaCollectionNewDetalleFactura = em.merge(oldCodigoFacturaOfDetalleFacturaCollectionNewDetalleFactura);
                    }
                }
            }
            em.getTransaction().commit();
        } catch (Exception ex) {
            String msg = ex.getLocalizedMessage();
            if (msg == null || msg.length() == 0) {
                Integer id = facturasCabecera.getCodigoFactura();
                if (findFacturasCabecera(id) == null) {
                    throw new NonexistentEntityException("The facturasCabecera with id " + id + " no longer exists.");
                }
            }
            throw ex;
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void destroy(Integer id) throws IllegalOrphanException, NonexistentEntityException {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            FacturasCabecera facturasCabecera;
            try {
                facturasCabecera = em.getReference(FacturasCabecera.class, id);
                facturasCabecera.getCodigoFactura();
            } catch (EntityNotFoundException enfe) {
                throw new NonexistentEntityException("The facturasCabecera with id " + id + " no longer exists.", enfe);
            }
            List<String> illegalOrphanMessages = null;
            Collection<DetalleFactura> detalleFacturaCollectionOrphanCheck = facturasCabecera.getDetalleFacturaCollection();
            for (DetalleFactura detalleFacturaCollectionOrphanCheckDetalleFactura : detalleFacturaCollectionOrphanCheck) {
                if (illegalOrphanMessages == null) {
                    illegalOrphanMessages = new ArrayList<String>();
                }
                illegalOrphanMessages.add("This FacturasCabecera (" + facturasCabecera + ") cannot be destroyed since the DetalleFactura " + detalleFacturaCollectionOrphanCheckDetalleFactura + " in its detalleFacturaCollection field has a non-nullable codigoFactura field.");
            }
            if (illegalOrphanMessages != null) {
                throw new IllegalOrphanException(illegalOrphanMessages);
            }
            Cliente codigoCliente = facturasCabecera.getCodigoCliente();
            if (codigoCliente != null) {
                codigoCliente.getFacturasCabeceraCollection().remove(facturasCabecera);
                codigoCliente = em.merge(codigoCliente);
            }
            em.remove(facturasCabecera);
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public List<FacturasCabecera> findFacturasCabeceraEntities() {
        return findFacturasCabeceraEntities(true, -1, -1);
    }

    public List<FacturasCabecera> findFacturasCabeceraEntities(int maxResults, int firstResult) {
        return findFacturasCabeceraEntities(false, maxResults, firstResult);
    }

    private List<FacturasCabecera> findFacturasCabeceraEntities(boolean all, int maxResults, int firstResult) {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            cq.select(cq.from(FacturasCabecera.class));
            Query q = em.createQuery(cq);
            if (!all) {
                q.setMaxResults(maxResults);
                q.setFirstResult(firstResult);
            }
            return q.getResultList();
        } finally {
            em.close();
        }
    }

    public FacturasCabecera findFacturasCabecera(Integer id) {
        EntityManager em = getEntityManager();
        try {
            return em.find(FacturasCabecera.class, id);
        } finally {
            em.close();
        }
    }

    public int getFacturasCabeceraCount() {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            Root<FacturasCabecera> rt = cq.from(FacturasCabecera.class);
            cq.select(em.getCriteriaBuilder().count(rt));
            Query q = em.createQuery(cq);
            return ((Long) q.getSingleResult()).intValue();
        } finally {
            em.close();
        }
    }

    public String obtenerUltimoNumeroFacttura() {
        EntityManager em = getEntityManager();

        Query q = em.createQuery("SELECT f.numeroFactura FROM FacturasCabecera f ORDER BY f.codigoFactura desc");

        q.setMaxResults(1);

        try {
            String numeroCompleto = (String) q.getSingleResult();
            String numeroSinF = numeroCompleto.substring(8); // Elimina los primeros dos caracteres "F_"
            long numero = Long.parseLong(numeroSinF);
            long siguienteNumero = numero + 1;
            return String.valueOf(siguienteNumero);
        } catch (NoResultException e) {
            return "000000001";
        }
    }

    public FacturasCabecera obtenerPorNumeroFactura(String numeroFactura) {
        EntityManager em = getEntityManager();

        Query q = em.createQuery("SELECT f FROM FacturasCabecera f WHERE f.numeroFactura = :numeroFactura");

        q.setParameter("numeroFactura", numeroFactura);

        return (FacturasCabecera) q.getSingleResult();
    }

}
